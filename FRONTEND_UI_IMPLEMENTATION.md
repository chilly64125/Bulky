# 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台 - UI 完整實現

## 📋 概述

本文檔說明系統已實現的三個主要 UI 層級：

1. **Guest UI（訪客界面）** - 公開登陸頁面
2. **Customer UI（客戶界面）** - 登入後的客戶儀表板
3. **Admin UI（管理者界面）** - 後台管理系統

---

## 🌍 1. Guest UI（訪客登陸頁面）

### 位置

- **文件**: `vue-frontend/src/views/GuestLandingView.vue`
- **路由**: `/` (根路徑)
- **要求**: 無需認證

### 功能特性

#### 英雄部分 (Hero Section)

- 項目名稱和簡介
- 兩個主要 CTA 按鈕：
  - 登入系統
  - 註冊帳號

#### 核心功能部分 (Features Section)

展示 4 大核心功能：

1. 祖先牌位管理 - 完整記錄祖先牌位資訊
2. 墓園塔位查詢 - 快速查詢懷恩塔塔位詳情
3. 文件管理 - 上傳和管理相關文件
4. 安全隱私 - 保護家族資訊

#### 快速開始部分 (Quick Links)

- 常見問題
- 聯絡客服
- 隱私政策
- 使用條款

#### 行動呼籲部分 (CTA Section)

- 邀請未註冊用戶加入系統

#### 頁尾 (Footer)

- 公司資訊和聯絡方式

### 設計特點

- 使用 Bootstrap 5 響應式設計
- 藍色漸變主題
- 響應式卡片和圖標
- 懸停效果增強用戶交互

---

## 👥 2. Customer UI（客戶儀表板）

### 位置

- **文件**: `vue-frontend/src/views/customer/DashboardView.vue`
- **路由**: `/app/customer` (已認證用戶)
- **要求**: 登入認證

### 功能特性

#### 頁面標題 (Page Header)

- 個性化歡迎信息（顯示用戶名）
- 個人設定按鈕

#### 快速統計卡片 (Stat Cards)

顯示 4 個關鍵統計：

1. 祖先牌位數量
2. 墓園塔位數量
3. 待處理訂單數
4. 帳號狀態

#### 主要內容區域 (Main Content)

**左側 - 快速操作 (Quick Actions)**
6 個快速操作按鈕：

- 新增祖先牌位
- 新增塔位紀錄
- 建立新訂單
- 查詢牌位
- 查看塔位
- 下載報表

**右側 - 最近活動 (Recent Activity)**

- 顯示最近 4 條活動記錄
- 包含活動圖標、描述和時間

#### 最近訂單表格 (Recent Orders)

- 訂單編號、金額、狀態、日期
- 狀態徽章（待確認、進行中、已完成）
- 查詢詳情按鈕

### 交互功能

```typescript
- formatCurrency() - 格式化台幣金額顯示
- formatDate() - 本地化日期格式
- viewOrderDetails() - 查看訂單詳情
- downloadReport() - 下載報表（待實現）
```

### 設計特點

- 卡片式佈局
- 懸停動畫效果
- 響應式網格系統
- 直觀的圖標和顏色編碼

---

## 🔐 3. Admin UI（管理者儀表板）

### 位置

- **文件**: `vue-frontend/src/views/admin/DashboardView.vue`
- **路由**: `/app/admin` (僅管理員可訪問)
- **要求**: 登入 + Admin 角色

### 功能特性

#### 統計摘要 (Statistics Cards)

4 個關鍵統計：

1. 會員總數
2. 管理員數量
3. 客戶數量
4. 其他角色數量

#### 管理功能卡片 (Admin Function Cards)

6 個主要管理區域，每個都有：

- 大型圖標
- 標題
- 簡短描述
- 導航按鈕

**管理功能列表:**

1. **會員管理** (People Icon)

   - 路由: `/app/admin/users`
   - 功能: 管理用戶、分配角色

2. **活動類別** (Tags Icon)

   - 路由: `/app/admin/category`
   - 功能: 管理活動分類

3. **宗親會基本檔** (Building Icon)

   - 路由: `/app/admin/company`
   - 功能: 管理宗親會組織資訊

4. **活動基本檔** (Box Icon)

   - 路由: `/app/admin/product`
   - 功能: 管理活動商品

5. **懷恩塔-塔位管理** (Building Check Icon)

   - 路由: `/app/admin/kindness`
   - 功能: 管理墓園塔位

6. **陳氏宗祠-牌位管理** (Houses Icon)
   - 路由: `/app/admin/ancestral`
   - 功能: 管理祖先牌位

#### 系統資訊 (System Information)

- 環境標示（開發/生產）
- 當前用戶的所有角色

### 交互功能

```typescript
- loadStats() - 從 API 加載用戶統計
- getRoleDisplayName() - 將角色代碼轉換為中文名稱
```

### 設計特點

- 明亮且易於導航
- 彩色編碼的管理區域
- 懸停視覺反饋
- 響應式卡片佈局

---

## 🔓 4. 登入錯誤處理

### 位置

- **文件**: `vue-frontend/src/views/auth/LoginView.vue`
- **路由**: `/login`

### 錯誤類型和消息

系統支持以下登入錯誤類型，每種都有特定的提示：

| 錯誤代碼              | 描述               | 用戶提示                                           |
| --------------------- | ------------------ | -------------------------------------------------- |
| `INVALID_CREDENTIALS` | 電子郵件或密碼錯誤 | "請檢查您的電子郵件和密碼是否正確。"               |
| `USER_NOT_FOUND`      | 電子郵件未註冊     | "此電子郵件尚未註冊。請建立新帳號。"               |
| `INVALID_PASSWORD`    | 密碼錯誤           | "密碼錯誤。請稍後重試，或重設密碼。"               |
| `ACCOUNT_LOCKED`      | 帳號被鎖定         | "帳號已被鎖定，請稍後再試或聯絡客服。"             |
| `ACCOUNT_DISABLED`    | 帳號被停用         | "此帳號已被停用。請聯絡客服以取得協助。"           |
| `EMAIL_NOT_CONFIRMED` | 電子郵件未驗證     | "您需要先驗證電子郵件。請檢查收件箱中的驗證信件。" |
| `NETWORK_ERROR`       | 網路連線失敗       | "網路連線失敗。請檢查您的網路連線並重試。"         |
| `SERVER_ERROR`        | 伺服器錯誤         | "伺服器出現問題。請稍後再試。"                     |

### 表單驗證

- 電子郵件格式驗證
- 密碼最小長度驗證（6 字符）
- 即時錯誤反饋
- 提交按鈕加載狀態

### UI 元素

- 加密的密碼輸入框
- 自動完成支持（username/password）
- 忘記密碼鏈接
- 註冊帳號鏈接
- 帶圖標的錯誤警告框

---

## 🚀 路由結構

### 未認證路由

```
/                    → Guest Landing Page (GuestLandingView)
/login               → Login Page (LoginView)
/register            → Register Page (RegisterView)
/welcome             → Welcome Page (GuestWelcomeView)
```

### 認證路由 (需要登入)

```
/app                 → App Layout (with Sidebar)
├── /                → Home Page (HomeView)
├── /customer        → Customer Dashboard (CustomerDashboardView)
├── /admin           → Admin Dashboard (DashboardView)
│   └── /users       → User Management (UsersManageView)
├── /ancestral       → Ancestral Position Management
├── /kindness        → Cemetery Position Management
├── /category        → Category Management
├── /company         → Company Management
├── /product         → Product Management
└── /order           → Order Management
```

---

## 🎨 設計系統

### 色彩方案

- **主色**: 藍色 (#0d6efd)
- **成功**: 綠色 (#198754)
- **警告**: 橙色 (#fd7e14)
- **危險**: 紅色 (#dc3545)
- **次要**: 紫色 (#6f42c1)
- **信息**: 青色 (#0dcaf0)

### 響應式斷點

- **XS** (< 576px): 手機
- **SM** (576px - 768px): 小平板
- **MD** (768px - 992px): 平板
- **LG** (992px - 1200px): 小屏幕
- **XL** (>= 1200px): 桌面

### 組件庫

- Bootstrap 5
- Bootstrap Icons
- Vue 3 Components

---

## 📱 響應式設計

所有 UI 都完全響應式：

1. **Guest Landing** - 一欄/多欄佈局
2. **Customer Dashboard** - 2/3/4 欄統計，智能重排
3. **Admin Dashboard** - 1/2/3 欄卡片佈局
4. **Login** - 居中容器，適應所有屏幕

---

## 🔄 集成點

### 需要後端實現的 API

```
GET /api/admin/users              - 獲取用戶列表
GET /api/admin/dashboard          - 獲取儀表板統計
PUT /api/admin/users/{id}/roles   - 更新用戶角色
DELETE /api/admin/users/{id}      - 刪除用戶
POST /auth/login                  - 用戶登入
```

### 認證流程

1. 用戶在 `/login` 輸入憑證
2. 提交至後端 API
3. 成功時保存令牌/會話
4. 重定向至 `/app` (認證區域)
5. 根據用戶角色控制可訪問的路由

---

## ✅ 測試檢查清單

### Guest Landing (`/`)

- [ ] 頁面加載無錯誤
- [ ] 所有按鈕正確鏈接
- [ ] 響應式設計在所有屏幕尺寸上工作
- [ ] 圖標和顏色正確顯示

### Login (`/login`)

- [ ] 表單驗證正常工作
- [ ] 錯誤消息正確顯示
- [ ] 成功登入後重定向至首頁
- [ ] 密碼字段正確隱藏文本

### Customer Dashboard (`/app/customer`)

- [ ] 顯示正確的用戶名
- [ ] 統計卡片加載數據
- [ ] 所有按鈕鏈接正確
- [ ] 最近活動和訂單顯示

### Admin Dashboard (`/app/admin`)

- [ ] 只有管理員可訪問
- [ ] 統計數據正確加載
- [ ] 所有管理功能卡片都可訪問
- [ ] 用戶角色正確顯示

---

## 🎯 下一步行動

1. **測試登入流程**

   - 使用有效的管理員帳號登入
   - 驗證重定向和數據加載

2. **連接後端 API**

   - 實現用戶統計端點
   - 測試數據獲取

3. **添加功能**

   - 實現報表下載
   - 添加個人設定頁面
   - 實現帳號設置

4. **性能優化**

   - 添加數據懶加載
   - 實現分頁

5. **安全性檢查**
   - 驗證角色基訪問控制 (RBAC)
   - 確保敏感數據加密

---

## 📚 相關文件

- Frontend 源代碼: `vue-frontend/src/`
- Router 配置: `vue-frontend/src/router/index.ts`
- 存儲: `vue-frontend/src/stores/authStore.ts`
- 組件: `vue-frontend/src/components/`

---

**最後更新**: 2025-12-03
**系統名稱**: 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台
**版本**: 1.0.0
