# VirtoCommerce Frontend - Developer Setup Guide

## 🚀 Quick Start for Frontend Development

This guide addresses common frontend setup issues and provides solutions for VirtoCommerce Storefront development.

## ⚠️ Common Frontend Issues & Solutions

### Issue 1: Yarn Version Conflicts
**Problem**: `packageManager: yarn@4.7.0... current global version is 1.22.22`  
**Cause**: Project requires Yarn 4.7.0 but system has older version  
**Solutions**:

```powershell
# Option 1: Enable Corepack (Requires Admin Privileges)
corepack enable
npm run dev

# Option 2: Use npm instead (Most reliable)
npm install
npx vite --host 0.0.0.0 --port 3000

# Option 3: Force Vite start (Skip yarn precheck)
npx vite --host 0.0.0.0 --port 3000 --force
```

### Issue 2: Process Instability
**Problem**: Frontend starts but processes stop/crash unexpectedly  
**Solution**: Run in separate PowerShell window:

```powershell
# Start in dedicated window to prevent crashes
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$(Get-Location)'; npx vite --host 0.0.0.0 --port 3000"

# Or manually open new PowerShell window and run:
npx vite --host 0.0.0.0 --port 3000
```

### Issue 3: HTTPS Certificate Warnings
**Problem**: `NET::ERR_CERT_AUTHORITY_INVALID` in browser  
**Explanation**: This is normal for development with self-signed certificates  
**Solution**: Click "Advanced" → "Proceed to localhost (unsafe)" in browser

### Issue 4: Backend Connection Errors
**Problem**: GraphQL connection refused or 404 errors  
**Cause**: Wrong backend URL in `.env.local` file  
**Solution**: Ensure correct backend URL:

```powershell
# Create/update .env.local file
"APP_BACKEND_URL=http://localhost:10645" | Out-File -FilePath .env.local -Encoding utf8

# Verify backend is running first
curl -I http://localhost:10645
```

## 🛠️ Step-by-Step Frontend Setup

### Prerequisites
- **Node.js 18+** (Download from nodejs.org)
- **VirtoCommerce Backend** running on http://localhost:10645
- **Git** for cloning

### Setup Steps

1. **Clone the repository:**
```powershell
git clone https://github.com/VirtoCommerce/vc-frontend.git
cd vc-frontend
```

2. **Configure environment:**
```powershell
# Create .env.local file with backend URL for local development
"APP_BACKEND_URL=http://localhost:10645" | Out-File -FilePath .env.local -Encoding utf8

# Verify content
Get-Content .env.local
# Should show: APP_BACKEND_URL=http://localhost:10645
```

3. **Install dependencies (choose one method):**

**Method A: Using npm (Recommended)**
```powershell
npm install
```

**Method B: Using yarn (if Corepack enabled)**
```powershell
corepack enable  # Requires admin privileges
npm run dev
```

4. **Start development server:**

**Option A: Direct Vite start (Most stable)**
```powershell
npx vite --host 0.0.0.0 --port 3000
```

**Option B: npm script (if yarn conflicts resolved)**
```powershell
npm run dev
```

**Option C: Separate PowerShell window (if crashes occur)**
```powershell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$(Get-Location)'; npx vite --host 0.0.0.0 --port 3000"
```

5. **Access the storefront:**
- URL: https://localhost:3000
- **Accept certificate warning** (normal for development)
- Should show VirtoCommerce storefront

## 🔧 Troubleshooting

### Yarn/npm Issues

**Error**: `packageManager field in package.json`
```powershell
# Ignore yarn requirement, use npm
npm install --legacy-peer-deps
npx vite --host 0.0.0.0 --port 3000
```

**Error**: `corepack enable` permission denied
```powershell
# Use npm instead
npm install
npx vite --host 0.0.0.0 --port 3000
```

### Process/Runtime Issues

**Error**: Process starts then stops immediately
```powershell
# Check if Node.js process is actually running
Get-Process node -ErrorAction SilentlyContinue

# If no processes found, use separate window approach
Start-Process powershell -ArgumentList "-NoExit", "-Command", "npx vite --host 0.0.0.0 --port 3000"
```

**Error**: Port 3000 already in use
```powershell
# Find and kill process using port 3000
netstat -ano | findstr ":3000"
Stop-Process -Id <ProcessId> -Force

# Or use different port
npx vite --host 0.0.0.0 --port 3001
```

### Backend Connection Issues

**Error**: GraphQL errors or connection refused
```powershell
# Verify backend is running
curl http://localhost:10645/health
# or visit: http://localhost:10645

# Check .env.local file has correct URL
Get-Content .env.local
# Should show: APP_BACKEND_URL=http://localhost:10645

# If backend URL is wrong, update it:
"APP_BACKEND_URL=http://localhost:10645" | Out-File -FilePath .env.local -Encoding utf8
```

**Error**: CORS errors in browser console
- This usually resolves automatically once both services are fully started
- Ensure backend is completely loaded before testing frontend

### HTTPS/Certificate Issues

**Error**: `NET::ERR_CERT_AUTHORITY_INVALID`
- This is normal for self-signed certificates in development
- Click "Advanced" in Chrome → "Proceed to localhost (unsafe)"
- Your connection is still encrypted, just using development certificates

**To disable HTTPS (not recommended):**
```javascript
// In vite.config.ts, modify server config:
server: {
  host: '0.0.0.0',
  port: 3000,
  https: false  // Add this line
}
```

## 📋 Quick Commands Cheat Sheet

```powershell
# Kill all Node.js processes
Stop-Process -Name "node" -Force -ErrorAction SilentlyContinue

# Quick start (most reliable method)
npm install
npx vite --host 0.0.0.0 --port 3000

# Check if frontend is running
Get-Process node -ErrorAction SilentlyContinue
netstat -an | findstr ":3000" | findstr "LISTENING"

# Test backend connection
curl -I http://localhost:10645

# View .env.local configuration
Get-Content .env.local
```

## 🎯 Environment Configuration

**Required Environment Variables:**
```bash
# .env.local file content
APP_BACKEND_URL=http://localhost:10645
```

**Optional Configurations:**
```bash
# For custom backend port
APP_BACKEND_URL=http://localhost:5000

# For production-like testing
APP_BACKEND_URL=https://your-backend-domain.com
```

## 🔍 Verification Steps

1. **Process Check:**
   ```powershell
   Get-Process node -ErrorAction SilentlyContinue
   # Should show node.exe process
   ```

2. **Port Check:**
   ```powershell
   netstat -an | findstr ":3000" | findstr "LISTENING"
   # Should show 0.0.0.0:3000 LISTENING
   ```

3. **Backend Connection:**
   ```powershell
   curl -I http://localhost:10645
   # Should return 200 OK
   ```

4. **Frontend Access:**
   - Open https://localhost:3000
   - Accept certificate warning
   - Should show VirtoCommerce storefront

## 🌐 Development URLs

| Service | URL | Purpose |
|---------|-----|---------|
| **Storefront** | https://localhost:3000 | Customer-facing store |
| **Backend API** | http://localhost:10645 | Admin panel & API |
| **GraphQL** | http://localhost:10645/graphql | GraphQL endpoint |

## 📚 Additional Resources

- [Frontend Documentation](https://docs.virtocommerce.org/storefront/)
- [Theme Development](https://docs.virtocommerce.org/storefront/developer-guide/theme-development/)
- [GraphQL API](https://docs.virtocommerce.org/platform/developer-guide/graphql-api/)
- [Platform Setup Guide](../vc-platform/DEVELOPER_SETUP.md)

## 🆘 Getting Help

If you encounter issues not covered here:

1. **Check Frontend Issues**: [Frontend GitHub Issues](https://github.com/VirtoCommerce/vc-frontend/issues)
2. **Platform Issues**: [Platform GitHub Issues](https://github.com/VirtoCommerce/vc-platform/issues)
3. **Documentation**: [docs.virtocommerce.org](https://docs.virtocommerce.org)
4. **Community**: [virtocommerce.org](https://www.virtocommerce.org) 