import { useEffect, useState } from 'react'
import { fetchCustomers, fetchCustomerOrders } from './api.js'
import './App.css'
import Loading from './components/Loading'
import ErrorMessage from './components/ErrorMessage'

function OrdersTable({ orders }) {
  if (!orders.length) return <div className="text-gray-500">No orders found for this customer.</div>

  // Calculate total sum
  const totalSum = orders.reduce((sum, order) => {
    let value = 0;
    if (typeof order.total === 'object' && order.total && typeof order.total.parsedValue === 'number') {
      value = order.total.parsedValue;
    } else if (typeof order.total === 'number') {
      value = order.total;
    }
    return sum + value;
  }, 0);

  return (
    <div className="w-full h-[80vh] overflow-y-auto overflow-x-auto">
      <table className="w-full bg-white border border-gray-300 text-base">
        <thead>
          <tr className="bg-gray-100 text-base">
            <th className="py-2 px-2 text-left border-b border-gray-200">Order ID</th>
            <th className="py-2 px-2 text-left border-b border-gray-200">Order Date</th>
            <th className="py-2 px-2 text-left border-b border-gray-200">Total</th>
            <th className="py-2 px-2 text-left border-b border-gray-200"># Products</th>
            <th className="py-2 px-2 text-left border-b border-gray-200">Warnings</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order, idx) => (
            <tr
              key={order.orderID || order.id}
              className={`border-b border-gray-100 hover:bg-blue-50 ${idx % 2 === 0 ? 'bg-navy-accent' : 'bg-navy-main'}`}
            >
              <td className="py-2 px-2 font-semibold">{order.orderID || order.id}</td>
              <td className="py-2 px-2">{order.orderDate ? order.orderDate.slice(0, 10) : order.date?.slice(0, 10)}</td>
              <td className="py-2 px-2">${typeof order.total === 'object' ? order.total.parsedValue?.toFixed(2) : order.total?.toFixed(2)}</td>
              <td className="py-2 px-2">{order.productCount}</td>
              <td className="py-2 px-2">
                {order.warning && (
                  <span className="text-xs text-yellow-700 bg-yellow-100 px-2 py-1 rounded border border-yellow-400">
                    {order.warning}
                  </span>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="flex justify-end mt-4 pr-4">
        <span className="text-lg font-bold text-white bg-navy-accent px-4 py-2 rounded shadow">Total Sum: ${totalSum.toFixed(2)}</span>
      </div>
    </div>
  )
}

function App() {
  const [customers, setCustomers] = useState([])
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [sideNavSearch, setSideNavSearch] = useState('')
  const [sideNavPage, setSideNavPage] = useState(1)
  const [selected, setSelected] = useState(null)
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(false)
  const [ordersLoading, setOrdersLoading] = useState(false)
  const [ordersError, setOrdersError] = useState(false)
  const CUSTOMERS_PER_PAGE = 25

  const loadCustomers = async () => {
    setLoading(true)
    setError(false)
    try {
      const data = await fetchCustomers()
      setCustomers(data)
    } catch {
      setError(true)
    } finally {
      setLoading(false)
    }
  }

  const loadOrders = async (customer) => {
    setOrdersLoading(true)
    setOrdersError(false)
    try {
      const data = await fetchCustomerOrders(customer.id || customer.customerID)
      setOrders(data)
    } catch {
      setOrdersError(true)
    } finally {
      setOrdersLoading(false)
    }
  }

  useEffect(() => { loadCustomers() }, [])
  useEffect(() => { if (selected) { loadOrders(selected) } else { setOrders([]) } }, [selected])

  const filtered = customers.filter(c => (c.name || c.companyName || '').toLowerCase().includes(search.toLowerCase()))
  const totalPages = Math.ceil(filtered.length / CUSTOMERS_PER_PAGE)
  const paginated = filtered.slice((page - 1) * CUSTOMERS_PER_PAGE, page * CUSTOMERS_PER_PAGE)

  const sideNavFiltered = customers.filter(c => (c.name || c.companyName || '').toLowerCase().includes(sideNavSearch.toLowerCase()))
  const sideNavTotalPages = Math.ceil(sideNavFiltered.length / CUSTOMERS_PER_PAGE)
  const sideNavPaginated = sideNavFiltered.slice((sideNavPage - 1) * CUSTOMERS_PER_PAGE, sideNavPage * CUSTOMERS_PER_PAGE)

  useEffect(() => { setSideNavPage(1) }, [sideNavSearch])
  useEffect(() => { if (!selected) { setSideNavSearch(''); setSideNavPage(1) } }, [selected])

  return (
    <div className="min-h-screen bg-gray-50 flex">
      {selected && (
        <div className="h-screen bg-white border-r border-gray-200 shadow-md flex flex-col overflow-hidden z-20" style={{ width: '30vw', minWidth: 240, maxWidth: 800 }}>
          <h2 className="text-xl font-bold mb-4">Customers</h2>
          <input
            className="mb-4 p-2 border rounded w-full"
            placeholder="Search customers..."
            value={sideNavSearch}
            onChange={e => setSideNavSearch(e.target.value)}
          />
          <div className="flex-1 min-h-0">
            <table className="w-full bg-white text-base">
              <thead>
                <tr className="bg-gray-100">
                  <th className="py-2 px-3 text-left">Name</th>
                  <th className="py-2 px-3 text-left">Orders</th>
                </tr>
              </thead>
              <tbody>
                {sideNavPaginated.map((c, idx) => {
                  const isSelected = selected && (selected.id || selected.customerID) === (c.id || c.customerID);
                  let rowClass = '';
                  if (isSelected) {
                    rowClass = 'bg-blue-600 text-white font-bold border-l-4 border-blue-800';
                  } else {
                    rowClass = 'hover:bg-blue-100 ' + (idx % 2 === 0 ? 'bg-navy-accent' : 'bg-navy-main');
                  }
                  return (
                    <tr
                      key={c.id || c.customerID}
                      className={`cursor-pointer transition-colors border-b border-gray-100 ${rowClass}`}
                      onClick={() => setSelected(c)}
                    >
                      <td className={`py-2 px-3 ${isSelected ? 'bg-blue-600 text-white rounded font-extrabold border border-blue-700' : ''}`}>
                        {c.name || c.companyName}
                      </td>
                      <td className="py-2 px-3">{c.orderCount ?? c.orders?.length ?? '-'}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
          {sideNavTotalPages > 1 && (
            <div className="flex justify-center items-center gap-2 mt-4">
              <button
                className="px-3 py-1 rounded border bg-gray-100 hover:bg-gray-200 disabled:opacity-50"
                onClick={() => setSideNavPage(sideNavPage - 1)}
                disabled={sideNavPage === 1}
              >
                Previous
              </button>
              <span className="mx-2">Page {sideNavPage} of {sideNavTotalPages}</span>
              <button
                className="px-3 py-1 rounded border bg-gray-100 hover:bg-gray-200 disabled:opacity-50"
                onClick={() => setSideNavPage(sideNavPage + 1)}
                disabled={sideNavPage === sideNavTotalPages}
              >
                Next
              </button>
            </div>
          )}
        </div>
      )}
      <div className={`flex-1 flex ${selected ? 'justify-center' : 'justify-end'} items-center`} style={selected ? { width: '70vw' } : {}}>
        <div className={`w-full max-w-5xl bg-white rounded shadow-md p-8 ${!selected ? 'ml-auto' : ''}`}>
          {!selected ? (
            <>
              <h1 className="text-2xl font-bold mb-4">Customers</h1>
              <input
                className="mb-4 p-2 border rounded w-full max-w-xs"
                placeholder="Search customers..."
                value={search}
                onChange={e => { setSearch(e.target.value); setPage(1); }}
              />
              {error && <ErrorMessage onRetry={loadCustomers} />}
              {loading && <Loading />}
              {!loading && !error && (
                <div className="overflow-x-auto flex-1">
                  <table className="w-full min-w-full bg-white shadow rounded border-2 border-gray-400 text-lg">
                    <thead>
                      <tr className="bg-gray-100 text-xl">
                        <th className="py-3 px-5 text-left border-b-2 border-gray-300">Customer Name</th>
                        <th className="py-3 px-5 text-left border-b-2 border-gray-300">Number of Orders</th>
                      </tr>
                    </thead>
                    <tbody>
                      {paginated.map((c, idx) => {
                        const isSelected = selected && (selected.id || selected.customerID) === (c.id || c.customerID);
                        let rowClass = '';
                        if (isSelected) {
                          rowClass = 'bg-blue-200 font-bold border-l-4 border-blue-600';
                        } else {
                          rowClass = idx % 2 === 0 ? 'bg-navy-accent' : 'bg-navy-main';
                        }
                        return (
                          <tr
                            key={c.id || c.customerID}
                            className={`cursor-pointer hover:bg-blue-100 transition-colors ${rowClass}`}
                            onClick={() => setSelected(c)}
                          >
                            <td className="py-3 px-5 font-semibold">{c.name || c.companyName}</td>
                            <td className="py-3 px-5">{c.orderCount ?? c.orders?.length ?? '-'}</td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              )}
              {totalPages > 1 && (
                <div className="flex justify-center items-center gap-2 mt-6">
                  <button
                    className="px-3 py-1 rounded border bg-gray-100 hover:bg-gray-200 disabled:opacity-50"
                    onClick={() => setPage(page - 1)}
                    disabled={page === 1}
                  >
                    Previous
                  </button>
                  <span className="mx-2">Page {page} of {totalPages}</span>
                  <button
                    className="px-3 py-1 rounded border bg-gray-100 hover:bg-gray-200 disabled:opacity-50"
                    onClick={() => setPage(page + 1)}
                    disabled={page === totalPages}
                  >
                    Next
                  </button>
                </div>
              )}
            </>
          ) : (
            <>
              {ordersError && <ErrorMessage onRetry={() => loadOrders(selected)} />}
              {ordersLoading && <Loading />}
              {!ordersLoading && !ordersError && (
                <>
                  <h1 className="text-2xl font-bold mb-4">
                    Orders for <span className="font-extrabold">{selected.name || selected.companyName}</span>
                  </h1>
                  <OrdersTable orders={orders} />
                  <button className="mt-6 px-4 py-2 bg-gray-200 rounded hover:bg-gray-300" onClick={() => setSelected(null)}>
                    ← Back to Customers
                  </button>
                </>
              )}
            </>
          )}
        </div>
      </div>
    </div>
  )
}

export default App
