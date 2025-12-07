# Independent Backend & Frontend Launcher Scripts

This directory contains PowerShell scripts to manage the development environment without process interference.

## Scripts Overview

### 1. `launch-all.ps1` ⭐ (Recommended - Start Here)

**Master script that does everything in order:**

- Stops all running backend/frontend/Docker processes
- Starts backend in a new independent PowerShell window
- Starts frontend in a new independent PowerShell window

```powershell
cd D:\Git\VueChenClan
.\scripts\launch-all.ps1
```

**Result:**

- Original window shows clean completion message
- Backend runs in new window (dotnet watch on port 5064)
- Frontend runs in new window (Vite dev server on port 5173)
- All processes are independent and won't interfere

---

### 2. `stop-all-processes.ps1`

**Stops all running processes cleanly:**

- Kills any `dotnet` processes (backend)
- Kills any `node` processes (frontend)
- Stops Docker containers with `docker-compose down`

```powershell
.\scripts\stop-all-processes.ps1
```

**Use when:**

- You need to clean up before rebuilding
- Ports are stuck/in use
- You want a fresh start

---

### 3. `start-backend.ps1`

**Launches backend in independent window:**

- Opens new PowerShell window
- Runs `dotnet watch run` in `BulkyWeb/` directory
- Backend listens on `http://localhost:5064`

```powershell
.\scripts\start-backend.ps1
```

**Note:** Script closes after backend window opens. Backend window stays open until manually closed.

---

### 4. `start-frontend.ps1`

**Launches frontend in independent window:**

- Opens new PowerShell window
- Installs npm dependencies (if needed)
- Runs `npm run dev` in `vue-frontend/` directory
- Frontend dev server on `http://localhost:5173`

```powershell
.\scripts\start-frontend.ps1
```

**Note:** Script closes after frontend window opens. Frontend window stays open until manually closed.

---

## Quick Start (Recommended Workflow)

### First Time Setup

```powershell
cd D:\Git\VueChenClan
.\scripts\launch-all.ps1
```

### After Each Code Change

1. **If only frontend changed:** Just refresh browser (HMR enabled)
2. **If backend changed:**
   - Close backend window (Ctrl+C)
   - Restart: `.\scripts\start-backend.ps1`
3. **If both changed or you want fresh start:**
   ```powershell
   .\scripts\launch-all.ps1
   ```

### To Stop Everything

```powershell
.\scripts\stop-all-processes.ps1
```

---

## Troubleshooting

### "Port already in use"

```powershell
.\scripts\stop-all-processes.ps1
# Wait 2 seconds
.\scripts\launch-all.ps1
```

### Backend won't start

1. Verify .NET 8 SDK installed:
   ```powershell
   dotnet --version
   ```
2. Check if port 5064 is available:
   ```powershell
   netstat -ano | findstr :5064
   ```

### Frontend won't start

1. Verify Node.js 18+ installed:
   ```powershell
   node --version
   npm --version
   ```
2. Clear npm cache and reinstall:
   ```powershell
   cd vue-frontend
   npm cache clean --force
   npm install
   npm run dev
   ```

### Docker containers interfering

```powershell
docker-compose down
# Then run
.\scripts\launch-all.ps1
```

---

## Environment Behavior

| Process  | Window             | Port | Status                          |
| -------- | ------------------ | ---- | ------------------------------- |
| Backend  | Independent PS     | 5064 | `dotnet watch` (auto-reload)    |
| Frontend | Independent PS     | 5173 | `vite` dev server (HMR enabled) |
| Docker   | Stopped (optional) | -    | Use only when explicitly needed |

**Key Benefits:**
✓ No process interference  
✓ Independent window control  
✓ Clean process termination  
✓ Hot Module Reload (HMR) for both  
✓ Easy development workflow

---

## Advanced: Custom Commands

**Run backend with specific configuration:**

```powershell
cd D:\Git\VueChenClan\BulkyWeb
dotnet watch run --launch-profile Development
```

**Run frontend with specific port:**

```powershell
cd D:\Git\VueChenClan\vue-frontend
npm run dev -- --host 0.0.0.0 --port 5173
```

**Monitor port usage:**

```powershell
netstat -ano | findstr :5064
netstat -ano | findstr :5173
```

---

For more information, see `../DEPLOYMENT_GUIDE.md`
