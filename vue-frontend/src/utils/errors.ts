// API error handling
export class APIError extends Error {
  constructor(
    public status: number,
    public message: string,
    public code?: string
  ) {
    super(message)
    this.name = 'APIError'
  }
}

// Validation error
export class ValidationError extends Error {
  constructor(
    public field: string,
    public message: string
  ) {
    super(message)
    this.name = 'ValidationError'
  }
}

// Authentication error
export class AuthenticationError extends APIError {
  constructor(message: string = '未授權') {
    super(401, message, 'UNAUTHORIZED')
    this.name = 'AuthenticationError'
  }
}

// Authorization error
export class AuthorizationError extends APIError {
  constructor(message: string = '無權訪問') {
    super(403, message, 'FORBIDDEN')
    this.name = 'AuthorizationError'
  }
}

// Not found error
export class NotFoundError extends APIError {
  constructor(message: string = '資源未找到') {
    super(404, message, 'NOT_FOUND')
    this.name = 'NotFoundError'
  }
}

// Generic error handler
export function handleError(error: any): string {
  if (error instanceof APIError) {
    return error.message
  }
  if (error?.response?.data?.message) {
    return error.response.data.message
  }
  if (error?.message) {
    return error.message
  }
  return '發生未知錯誤'
}
