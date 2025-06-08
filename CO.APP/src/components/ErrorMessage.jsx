// src/components/ErrorMessage.jsx
export default function ErrorMessage({ onRetry }) {
  return (
    <div className="bg-red-100 text-red-700 p-4 rounded mb-4 flex flex-col items-center">
      <span>An error occurred. Please try again later.</span>
      <button className="mt-2 px-4 py-1 bg-red-600 text-white rounded" onClick={onRetry}>Retry</button>
    </div>
  )
}
