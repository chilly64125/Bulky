// API error codes
export const API_ERROR_CODES = {
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  CONFLICT: 409,
  INTERNAL_ERROR: 500,
} as const;

// HTTP status messages
export const HTTP_STATUS_MESSAGES: Record<number, string> = {
  400: "請求格式錯誤",
  401: "未授權，請重新登入",
  403: "無權訪問此資源",
  404: "資源未找到",
  409: "資源衝突",
  500: "伺服器內部錯誤",
  503: "服務暫時不可用",
};

// Application constants
export const APP_CONSTANTS = {
  AUTO_LOGOUT_MINUTES: 1,
  WARNING_BEFORE_LOGOUT_SECONDS: 5,
  DEFAULT_PAGE_SIZE: 20,
  MAX_FILE_SIZE_MB: 10,
} as const;

// User roles
export const USER_ROLES = {
  ADMIN: "Admin",
  CUSTOMER: "Customer",
  // Unauthenticated indicates a site visitor without login
  UNAUTHENTICATED: "Unauthenticated, ie :No-Login",
} as const;

// Grid dimensions
export const GRID_DIMENSIONS = {
  ANCESTRAL: {
    LEVELS: 10,
    POSITIONS: 10,
  },
  KINDNESS: {
    FLOORS: [1, 2, 3],
    SECTIONS: ["甲區", "乙區", "丙區", "丁區", "戊區", "己區"],
  },
} as const;

// Position status
export const POSITION_STATUS = {
  VACANT: "vacant",
  OCCUPIED: "occupied",
  RESERVED: "reserved",
} as const;
