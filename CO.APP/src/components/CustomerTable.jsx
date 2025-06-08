// src/components/CustomerTable.jsx
export default function CustomerTable({ customers, onSelect, selectedId, search, setSearch }) {
  return (
    <div className="overflow-x-auto">
      <input
        className="mb-4 p-2 border rounded w-full max-w-xs"
        placeholder="Search customers..."
        value={search}
        onChange={e => setSearch(e.target.value)}
      />
      <table className="min-w-full bg-white shadow rounded">
        <thead>
          <tr className="bg-gray-100">
            <th className="py-2 px-4 text-left">Customer Name</th>
            <th className="py-2 px-4 text-left">Number of Orders</th>
          </tr>
        </thead>
        <tbody>
          {customers.map(c => (
            <tr
              key={c.id || c.customerID}
              className={`cursor-pointer hover:bg-blue-100 transition-colors ${selectedId === (c.id || c.customerID) ? 'bg-blue-200 font-bold border-l-4 border-blue-600' : ''}`}
              onClick={() => onSelect(c)}
            >
              <td className="py-2 px-4">{c.name || c.companyName}</td>
              <td className="py-2 px-4">{c.orderCount ?? c.orders?.length ?? '-'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
