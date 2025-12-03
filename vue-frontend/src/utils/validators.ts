// Email validation
export function validateEmail(email: string): boolean {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return re.test(email)
}

// Phone validation (Taiwan format)
export function validatePhone(phone: string): boolean {
  const re = /^(\+886|0)[2-9]\d{7,9}$/
  return re.test(phone)
}

// ID validation (Taiwan ID)
export function validateTaiwanID(id: string): boolean {
  if (!id || id.length !== 10) return false
  const firstChar = id.charCodeAt(0)
  const checksum =
    ((firstChar - 64) * 10 +
      [...id.slice(1)].reduce((sum, char, idx) => sum + parseInt(char) * (9 - idx), 0)) %
    10
  return checksum === 0
}

// URL validation
export function validateURL(url: string): boolean {
  try {
    new URL(url)
    return true
  } catch {
    return false
  }
}

// Required field
export function validateRequired(value: any): boolean {
  if (value === null || value === undefined) return false
  if (typeof value === 'string') return value.trim().length > 0
  if (Array.isArray(value)) return value.length > 0
  return true
}

// Min length
export function validateMinLength(value: string, min: number): boolean {
  return value.length >= min
}

// Max length
export function validateMaxLength(value: string, max: number): boolean {
  return value.length <= max
}
