// API utility for customer and order endpoints
const API_BASE = 'https://localhost:5001';

// Patch: Map API customer fields to expected UI fields
export async function fetchCustomers() {
  const res = await fetch(`${API_BASE}/customers`);
  if (!res.ok) throw new Error('Failed to fetch customers');
  const data = await res.json();
  return data.map(c => ({
    id: c.customerID || c.id,
    name: c.companyName || c.name,
    orderCount: c.numberOfOrders ?? c.orderCount ?? c.orders?.length,
    ...c
  }));
}

export async function fetchCustomerOrders(customerId) {
  const res = await fetch(`${API_BASE}/customers/customer/${customerId}/orders`);
  if (!res.ok) throw new Error('Failed to fetch orders');
  const data = await res.json();
  // Map API fields to UI fields
  return data.map(o => ({
    id: o.orderID || o.id,
    date: o.orderDate || o.date,
    totalAmount: o.total?.parsedValue ?? o.total ?? 0,
    productCount: o.productCount ?? (o.products?.length ?? 0),
    warning: o.warning,
    ...o
  }));
}
