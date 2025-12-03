// Date formatters
export function formatDate(date: string | Date | null | undefined): string {
  if (!date) return '-'
  const d = new Date(date)
  return d.toLocaleDateString('zh-TW')
}

export function formatDateTime(date: string | Date | null | undefined): string {
  if (!date) return '-'
  const d = new Date(date)
  return d.toLocaleString('zh-TW')
}

export function formatTime(date: string | Date | null | undefined): string {
  if (!date) return '-'
  const d = new Date(date)
  return d.toLocaleTimeString('zh-TW')
}

// Currency formatter
export function formatCurrency(amount: number, currency: string = 'TWD'): string {
  return new Intl.NumberFormat('zh-TW', {
    style: 'currency',
    currency
  }).format(amount)
}

// String utilities
export function truncate(text: string, length: number = 50): string {
  if (!text) return ''
  return text.length > length ? text.substring(0, length) + '...' : text
}

export function capitalize(text: string): string {
  if (!text) return ''
  return text.charAt(0).toUpperCase() + text.slice(1)
}

// Array utilities
export function unique<T>(arr: T[], key?: keyof T): T[] {
  if (!key) return [...new Set(arr)]
  const seen = new Set()
  return arr.filter(item => {
    const value = item[key]
    if (seen.has(value)) return false
    seen.add(value)
    return true
  })
}

export function groupBy<T>(arr: T[], key: keyof T): Map<any, T[]> {
  const grouped = new Map()
  for (const item of arr) {
    const value = item[key]
    if (!grouped.has(value)) {
      grouped.set(value, [])
    }
    grouped.get(value).push(item)
  }
  return grouped
}
