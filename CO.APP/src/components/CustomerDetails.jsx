import { useEffect, useState } from 'react'
import { fetchCustomerOrders } from '../api.js'
import Loading from './Loading'
import ErrorMessage from './ErrorMessage'

export default function CustomerDetails({ customer }) {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(false)

  const loadOrders = async () => {
    setLoading(true)
    setError(false)
    try {
      const data = await fetchCustomerOrders(customer.id || customer.customerID)
      setOrders(data)
    } catch {
      setError(true)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    loadOrders()
    // eslint-disable-next-line
  }, [customer.id, customer.customerID])

  if (loading) return <Loading />
  if (error) return <ErrorMessage onRetry={loadOrders} />

  if (!orders.length) return (
    <div className="w-full flex flex-col items-center justify-center h-full">
      <h2 className="text-2xl font-bold mb-4">{customer.name || customer.companyName}</h2>
      <div className="text-gray-500">No orders found for this customer.</div>
    </div>
  )

  return (
    <div className="w-full flex flex-col items-center justify-center h-full">
      <h2 className="text-2xl font-bold mb-6 text-center">{customer.name || customer.companyName}</h2>
      <div className="overflow-x-auto w-full">
        <table className="min-w-full bg-white shadow rounded">
          <thead>
            <tr className="bg-gray-100">
              <th className="py-2 px-4 text-left">Order ID</th>
              <th className="py-2 px-4 text-left">Order Date</th>
              <th className="py-2 px-4 text-left">Total Amount</th>
              <th className="py-2 px-4 text-left"># Products</th>
              <th className="py-2 px-4 text-left">Warnings</th>
            </tr>
          </thead>
          <tbody>
            {orders.map(order => (
              <tr key={order.orderID || order.id} className="border-b last:border-b-0">
                <td className="py-2 px-4">{order.orderID || order.id}</td>
                <td className="py-2 px-4">{order.orderDate ? order.orderDate.slice(0, 10) : order.date?.slice(0, 10)}</td>
                <td className="py-2 px-4">${typeof order.total === 'object' ? order.total.parsedValue?.toFixed(2) : order.total?.toFixed(2)}</td>
                <td className="py-2 px-4">{order.productCount}</td>
                <td className="py-2 px-4">
                  {order.warning && (
                    <span className="text-xs text-yellow-700 bg-yellow-100 px-2 py-1 rounded">
                      {order.warning}
                    </span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}
