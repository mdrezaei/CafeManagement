# Cafe Management System

A microservices-based cafe management system built with ASP.NET Core, following Domain-Driven Design (DDD) and Event-Driven Architecture.

**Author:** MohamadReza Dadrezaei
**Repository:** [github.com/mdrezaei/CafeManagement](https://github.com/mdrezaei/CafeManagement.git)

---

## Overview

This system automates the cafe workflow through two independent microservices:

- **MenuAndOrdering.API** — customer-facing service (menu browsing, order placement, order tracking)
- **CafeManagement.API** — staff-facing service (order management, tables, menu, employees)

The two services communicate asynchronously using the Outbox Pattern, ensuring reliable event delivery without requiring an external message broker.

---

## Key Features

- Microservices architecture with independent databases (logical isolation via schemas)
- Domain-Driven Design with Aggregates, Entities, and Value Objects
- Event-Driven communication using the Outbox Pattern
- JWT-based authentication for staff and customers
- QR code generation for each table
- Customer order flow via QR scan
- Staff dashboard for orders, tables, and menu management
- Idempotent event handling to prevent duplicate orders
- Password hashing with BCrypt

---

## Tech Stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT (JSON Web Tokens)
- BCrypt.Net-Next
- QRCoder
- HTML / CSS / JavaScript (frontend)

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (local or remote)
- Visual Studio 2022+ or VS Code

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/mdrezaei/CafeManagement.git
   ```

2. Configure connection strings and JWT key using environment variables or `launchSettings.json`:
   - `ConnectionStrings__DefaultConnection`
   - `Jwt__Key`

3. Apply database schema using the SQL scripts in `database/`:
   - `admin-schema.sql` for CafeManagement.API
   - `menu-schema.sql` for MenuAndOrdering.API

4. Run both projects (multiple startup projects in Visual Studio):
   - MenuAndOrdering.API
   - CafeManagement.API

5. Access the applications:
   - Customer menu: `https://localhost:7205/menu.html?tableId=1`
   - Admin panel: `https://localhost:7256/login.html`

---

## Configuration

Sensitive values are not stored in the repository. Set them via environment variables:

| Variable | Purpose |
|----------|---------|
| `ConnectionStrings__DefaultConnection` | Database connection string |
| `Jwt__Key` | JWT signing key (must match in both services) |
| `AppBaseUrl` | Public URL of the customer service |
| `MenuAndOrderingBaseUrl` | Customer service URL for Outbox sync |
| `CafeManagementBaseUrl` | Management service URL for Outbox sync |

---

## Database

Both services share a single SQL Server instance with separate schemas:

- `admin.*` — tables for CafeManagement.API
- `menu.*` — tables for MenuAndOrdering.API

Schema scripts are provided in the `database/` directory.

---

## API Overview

### Customer Service (MenuAndOrdering.API)

- `GET /api/menu` — list menu items
- `POST /api/order` — place an order
- `GET /api/order/track` — track today's orders by phone number
- `GET /api/table/{id}` — validate a table

### Management Service (CafeManagement.API)

- `POST /api/auth/employee/login` — staff login
- `POST /api/auth/customer/login` — customer login
- `GET /api/admin/MOrder` — list orders
- `PATCH /api/admin/MOrder/{id}/status` — update order status
- `GET /api/admin/MTable/{id}/qrcode` — generate table QR code

---

## License

MIT

---

# سیستم مدیریت کافه

یک سیستم مدیریت کافه مبتنی بر میکروسرویس، ساخته‌شده با ASP.NET Core، بر پایه‌ی طراحی دامنه‌محور (DDD) و معماری رویدادمحور.

**نویسنده:** محمدرضا دادرضایی
**مخزن:** [github.com/mdrezaei/CafeManagement](https://github.com/mdrezaei/CafeManagement.git)

---

## مرور کلی

این سیستم فرایند کاری کافه را از طریق دو میکروسرویس مستقل خودکار می‌کند:

- **MenuAndOrdering.API** — سرویس مشتری (مشاهده منو، ثبت سفارش، پیگیری سفارش)
- **CafeManagement.API** — سرویس کارکنان (مدیریت سفارش، میزها، منو، کارکنان)

ارتباط بین دو سرویس به‌صورت غیرهمزمان و با استفاده از الگوی Outbox انجام می‌شود تا از تحویل مطمئن رویدادها بدون نیاز به کارگزار پیام خارجی اطمینان حاصل شود.

---

## ویژگی‌های کلیدی

- معماری میکروسرویس با پایگاه‌های داده‌ی مستقل منطقی (از طریق طرح‌واره‌ها)
- طراحی دامنه‌محور با موجودیت‌ها، اشیای مقداری و ریشه‌های تجمع
- ارتباط رویدادمحور با الگوی Outbox
- احراز هویت مبتنی بر JWT برای کارکنان و مشتریان
- تولید رمزینه پاسخ سریع برای هر میز
- جریان سفارش مشتری از طریق اسکن QR
- داشبورد کارکنان برای مدیریت سفارش، میز و منو
- پردازش ناتوان‌ساز رویدادها برای جلوگیری از ثبت سفارش تکراری
- درهم‌سازی رمز عبور با BCrypt

---

## فناوری‌ها

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT
- BCrypt.Net-Next
- QRCoder
- HTML / CSS / JavaScript

---

## راه‌اندازی

### پیش‌نیازها

- .NET 10 SDK
- SQL Server (لوکال یا راه دور)
- Visual Studio 2022 یا بالاتر

### مراحل

۱. کلون کردن مخزن:
   ```bash
   git clone https://github.com/mdrezaei/CafeManagement.git
   ```

۲. تنظیم رشته اتصال و کلید JWT از طریق متغیرهای محیطی یا `launchSettings.json`:
   - `ConnectionStrings__DefaultConnection`
   - `Jwt__Key`

۳. اجرای اسکریپت‌های SQL موجود در پوشه‌ی `database/`:
   - `admin-schema.sql` برای CafeManagement.API
   - `menu-schema.sql` برای MenuAndOrdering.API

۴. اجرای هر دو پروژه به‌صورت همزمان:
   - MenuAndOrdering.API
   - CafeManagement.API

۵. دسترسی به برنامه‌ها:
   - منوی مشتری: `https://localhost:7205/menu.html?tableId=1`
   - پنل مدیریت: `https://localhost:7256/login.html`

---

## پیکربندی

مقادیر حساس در مخزن ذخیره نمی‌شوند. آن‌ها را از طریق متغیرهای محیطی تنظیم کنید:

| متغیر | کاربرد |
|-------|--------|
| `ConnectionStrings__DefaultConnection` | رشته اتصال پایگاه داده |
| `Jwt__Key` | کلید امضای JWT (باید در هر دو سرویس یکسان باشد) |
| `AppBaseUrl` | آدرس عمومی سرویس مشتری |
| `MenuAndOrderingBaseUrl` | آدرس سرویس مشتری برای همگام‌سازی Outbox |
| `CafeManagementBaseUrl` | آدرس سرویس مدیریت برای همگام‌سازی Outbox |

---

## پایگاه داده

هر دو سرویس از یک نمونه SQL Server با طرح‌واره‌های مجزا استفاده می‌کنند:

- `admin.*` — جدول‌های CafeManagement.API
- `menu.*` — جدول‌های MenuAndOrdering.API

اسکریپت‌های ساخت طرح‌واره در پوشه‌ی `database/` ارائه شده‌اند.

---

## نمای کلی API

### سرویس مشتری (MenuAndOrdering.API)

- `GET /api/menu` — فهرست آیتم‌های منو
- `POST /api/order` — ثبت سفارش
- `GET /api/order/track` — پیگیری سفارش‌های امروز با شماره تلفن
- `GET /api/table/{id}` — اعتبارسنجی میز

### سرویس مدیریت (CafeManagement.API)

- `POST /api/auth/employee/login` — ورود کارکنان
- `POST /api/auth/customer/login` — ورود مشتریان
- `GET /api/admin/MOrder` — فهرست سفارش‌ها
- `PATCH /api/admin/MOrder/{id}/status` — تغییر وضعیت سفارش
- `GET /api/admin/MTable/{id}/qrcode` — تولید رمزینه میز

---

## مجوز

MIT
