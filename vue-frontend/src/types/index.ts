/**
 * Type definitions for the 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台 application
 */

export interface User {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
  isActive: boolean;
  lastLoginAt?: Date;
}

export interface AuthToken {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  data?: {
    user: User;
    token: AuthToken;
  };
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  passwordConfirmation: string;
  firstName: string;
  lastName: string;
}

// Ancestral Types
export interface AncestralPosition {
  ancestralPositionId: number;
  positionId: string; // Format: "側-區-層:編號"
  side: string; // "L" or "R"
  section: string; // "甲區", "乙區", etc.
  level: number;
  position: number;
  name?: string;
  occupantName?: string;
  occupantPhone?: string;
  dateRegistered?: Date;
  notes?: string;
}

export interface AncestralPositionQuery {
  side?: string;
  section?: string;
  level?: number;
  positionId?: string;
  occupantName?: string;
  page?: number;
  pageSize?: number;
}

export interface AncestralLayoutConfig {
  Layout_L: string;
  Layout_R: string;
  Side: number;
  Section: number;
  Level: number;
  Position: number;
  [key: string]: any;
}

// Kindness Types
export interface KindnessPosition {
  kindnessPositionId: number;
  positionId: string;
  floor: number; // 1, 2, or 3
  section: string; // "甲區", "乙區", etc.
  row: number;
  column: number;
  occupantName?: string;
  occupantPhone?: string;
  dateRegistered?: Date;
  notes?: string;
}

export interface KindnessPositionQuery {
  floor?: number;
  section?: string;
  row?: number;
  column?: number;
  occupantName?: string;
  page?: number;
  pageSize?: number;
}

export interface KindnessLayoutConfig {
  Layout_1F: string;
  Layout_2F: string;
  Layout_3F: string;
  Floor: number;
  Section: number;
  Level_1f_2f: number;
  Level_3f: number;
  Position: number;
  [key: string]: any;
}

// Category Type
export interface Category {
  categoryId: number;
  name: string;
  displayOrder: number;
  isActive: boolean;
  createdAt: Date;
}

// Company Type
export interface Company {
  companyId: number;
  name: string;
  address?: string;
  city?: string;
  state?: string;
  phone?: string;
  email?: string;
  isActive: boolean;
  createdAt: Date;
}

// Product Type
export interface Product {
  id: number;
  productId?: number;
  title: string;
  description?: string;
  isbn?: string;
  categoryId: number;
  category?: Category;
  companyId: number;
  company?: Company;
  hDate?: string;
  heldYN: string;
  listPrice: number;
  price?: number;
  imageUrl?: string;
  productImages?: ProductImage[];
  isActive?: boolean;
  createdAt?: Date;
}

export interface ProductImage {
  id: number;
  productId: number;
  imageUrl: string;
}

// Order Type
export interface Order {
  orderId: number;
  userId: string;
  orderDate: Date;
  totalPrice: number;
  status: string; // "Pending", "Processing", "Shipped", "Delivered", "Cancelled"
  paymentStatus: string; // "Pending", "Paid", "Failed"
  items: OrderDetail[];
}

export interface OrderDetail {
  orderDetailId: number;
  orderId: number;
  productId: number;
  quantity: number;
  price: number;
}

// Event Registration Type
export interface EventRegistration {
  eventRegistrationId: number;
  userId: string;
  productId: number;
  registrationDate: Date;
  status: string; // "Registered", "Attended", "Cancelled"
  notes?: string;
}

// Survey Type
export interface SurveyResponse {
  surveyResponseId: number;
  userId: string;
  question1: string;
  question2: string;
  question3: string;
  submittedAt: Date;
}

// API Response Wrapper
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: Record<string, string[]>;
}

export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// Logout Duration Config
export interface LogoutDurationConfig {
  AUTO_LOGOUT_MINUTE: number;
  WARNING_BEFORE_LOGOUT_SECOND: number;
}

export interface AppConfig {
  Kindness: KindnessLayoutConfig;
  Ancestral: AncestralLayoutConfig;
  Logout_Duration: LogoutDurationConfig;
  Work_Duration: number;
  Import_Duration: number;
  WORK_WARNING_SECONDS: number;
  PublishDate: string;
}

// Notification Types
export interface Toast {
  id: string;
  message: string;
  type: "success" | "error" | "info" | "warning";
  duration?: number;
}

// Permission Check
export interface PermissionCheck {
  hasPermission: boolean;
  requiredRoles?: string[];
  userRoles?: string[];
}
