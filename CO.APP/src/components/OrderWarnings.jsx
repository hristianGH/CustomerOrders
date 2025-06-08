// src/components/OrderWarnings.jsx
export default function OrderWarnings({ order }) {
  if (order.hasDiscontinuedProducts || order.hasStockIssues) {
    return (
      <span className="ml-2 text-xs text-yellow-700 bg-yellow-100 px-2 py-1 rounded">
        {order.hasDiscontinuedProducts && 'Discontinued products'}
        {order.hasDiscontinuedProducts && order.hasStockIssues && ', '}
        {order.hasStockIssues && 'Stock issues'}
      </span>
    )
  }
  return null
}
