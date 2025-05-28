# VirtoCommerce Platform - Developer Setup Guide

## 🚀 Quick Start for Developers

This guide addresses common setup issues and provides step-by-step instructions to get VirtoCommerce Platform running locally.

## ⚠️ Common Issues & Solutions

### Issue 1: "Unable to run your project" (Docker Compose Error)
**Problem**: Running `dotnet run` from wrong directory shows Docker Compose error  
**Cause**: VirtoCommerce has multiple projects; running from root triggers Docker Compose mode  
**Solution**: Always run from the web project directory:

```bash
# ❌ Wrong (from root directory - triggers Docker Compose error)
D:\work\vc-platform> dotnet run

# ✅ Correct (from web project directory)  
D:\work\vc-platform\src\VirtoCommerce.Platform.Web> dotnet run
```

### Issue 2: PowerShell Syntax Errors on Windows
**Problem**: Using bash syntax (`&&`) in PowerShell  
**Solution**: Use PowerShell syntax:

```powershell
# ❌ Wrong (bash syntax)
cd src\VirtoCommerce.Platform.Web && dotnet run

# ✅ Correct (PowerShell syntax)
cd src\VirtoCommerce.Platform.Web; dotnet run
```

### Issue 3: Port Conflicts
**Problem**: "Address already in use" errors on startup  
**Solution**: Kill existing processes:

```powershell
# Check what's using the port
netstat -ano | findstr ":10645"

# Kill the specific process (replace XXXX with Process ID)
Stop-Process -Id XXXX -Force

# Or kill all dotnet processes
Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue
```

## 🛠️ Step-by-Step Setup

### Prerequisites
- **.NET 8 SDK** (Download from Microsoft)
- **Node.js 18+** (For frontend development)
- **SQL Server** (Local installation or Docker)
- **Git** (For cloning repositories)

### Backend Setup

1. **Clone and navigate to the CORRECT directory:**
```bash
git clone https://github.com/VirtoCommerce/vc-platform.git
cd vc-platform

# ⚠️ CRITICAL: Navigate to the web project directory
cd src/VirtoCommerce.Platform.Web
```

2. **Setup SQL Server:**

**Option A: Docker (Recommended)**
```bash
docker run -d --name vc-sqlserver \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_PID=Express" \
  -e "SA_PASSWORD=Pass@word" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:latest
```

**Option B: Local SQL Server**
Update `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "VirtoCommerce": "Data Source=localhost,1433;Initial Catalog=VirtoCommerce3;User ID=sa;Password=Pass@word;TrustServerCertificate=True;"
  }
}
```

3. **Build the project:**
```bash
# From root directory
dotnet build VirtoCommerce.Platform.sln
```

4. **Run the backend:**
```bash
# ⚠️ IMPORTANT: Must be run from src/VirtoCommerce.Platform.Web directory
cd src/VirtoCommerce.Platform.Web  # If not already there
dotnet run --environment Development

# Alternative: Run with project path from any directory
dotnet run --project "src\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.Web.csproj" --environment Development
```

5. **Verify backend is running:**
- URL: http://localhost:10645
- Login: `admin` / `store`
- You should see the admin dashboard

### Frontend Setup

1. **Clone frontend repository:**
```bash
# In a separate directory
git clone https://github.com/VirtoCommerce/vc-frontend.git
cd vc-frontend
```

2. **Configure backend connection:**
```bash
# Create .env file with correct backend URL
echo APP_BACKEND_URL=http://localhost:10645 > .env
```

3. **Handle Yarn version conflicts:**

The frontend requires Yarn 4.7.0, but most systems have Yarn 1.x. Here are solutions:

**Option A: Enable Corepack (Requires Admin Privileges)**
```bash
corepack enable
npm run dev
```

**Option B: Use npm instead (If Corepack fails)**
```bash
npm install
npx vite --host 0.0.0.0 --port 3000
```

**Option C: Bypass yarn precheck**
```bash
npx vite --host 0.0.0.0 --port 3000 --force
```

4. **Start frontend (Handle Process Instability):**

The frontend process can be unstable in some environments. Use this approach:

```powershell
# Start in separate PowerShell window to prevent crashes
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$(pwd)'; npx vite --host 0.0.0.0 --port 3000"
```

5. **Access storefront:**
- URL: https://localhost:3000
- ⚠️ **Certificate Warning**: Click "Advanced" → "Proceed to localhost (unsafe)"
- This is normal for self-signed HTTPS certificates in development

## 🔧 Troubleshooting

### Backend Issues

**Error**: "Unable to run your project... OutputType is 'DockerCompose'"
```bash
# You're in the wrong directory! Navigate to the web project:
cd src/VirtoCommerce.Platform.Web
dotnet run --environment Development
```

**Error**: "Address already in use" on port 10645
```bash
# Find and kill the process using the port
netstat -ano | findstr ":10645"
Stop-Process -Id <ProcessId> -Force

# Then restart from correct directory
cd src/VirtoCommerce.Platform.Web
dotnet run --environment Development
```

**Error**: Database connection issues
```bash
# Verify SQL Server is running
docker ps  # If using Docker
# Or check SQL Server services in Windows

# Test connection string in appsettings.Development.json
```

### Frontend Issues

**Error**: `packageManager: yarn@4.7.0... current global version is 1.22.22`
```bash
# Solution 1: Enable Corepack (requires admin)
corepack enable

# Solution 2: Use npm instead
npm install
npx vite --host 0.0.0.0 --port 3000

# Solution 3: Force vite start
npx vite --host 0.0.0.0 --port 3000 --force
```

**Error**: Frontend shows as running but browser can't connect
```bash
# Check if Node.js processes are actually running
Get-Process node -ErrorAction SilentlyContinue

# If no processes, restart in separate window
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'path-to-frontend'; npx vite --host 0.0.0.0 --port 3000"
```

**Error**: "NET::ERR_CERT_AUTHORITY_INVALID" in browser
- This is expected for development with self-signed certificates
- Click "Advanced" → "Proceed to localhost (unsafe)"
- Your connection is still encrypted, just using self-signed certs

### General Issues

**Services stop unexpectedly:**
```bash
# Monitor running processes
Get-Process dotnet,node -ErrorAction SilentlyContinue

# Check listening ports
netstat -an | findstr ":10645\|:3000" | findstr "LISTENING"

# Restart services if needed
# Backend: (must be from src/VirtoCommerce.Platform.Web)
dotnet run --environment Development

# Frontend: 
npx vite --host 0.0.0.0 --port 3000
```

## 📋 Quick Commands Cheat Sheet

```bash
# Kill all development processes
Stop-Process -Name "dotnet","node" -Force -ErrorAction SilentlyContinue

# Start backend (⚠️ from correct directory!)
cd src/VirtoCommerce.Platform.Web
dotnet run --environment Development

# Start frontend
cd vc-frontend
npx vite --host 0.0.0.0 --port 3000

# Check what's running
Get-Process dotnet,node -ErrorAction SilentlyContinue
netstat -an | findstr ":10645\|:3000" | findstr "LISTENING"

# Test services
# Backend: curl http://localhost:10645
# Frontend: Open https://localhost:3000 in browser
```

## 🎯 Final Access URLs

Once everything is running successfully:

| Service | URL | Credentials | Notes |
|---------|-----|-------------|-------|
| **Backend Admin** | http://localhost:10645 | admin/store | Admin dashboard |
| **Frontend Store** | https://localhost:3000 | N/A | Accept certificate warning |
| **SQL Server** | localhost:1433 | sa/Pass@word | If using Docker |

## 🔍 Verification Steps

1. **Backend Check:**
   ```bash
   # Should return 200 OK
   curl -I http://localhost:10645
   ```

2. **Frontend Check:**
   ```bash
   # Check processes are running
   Get-Process node -ErrorAction SilentlyContinue
   
   # Check port is listening
   netstat -an | findstr ":3000" | findstr "LISTENING"
   ```

3. **Database Check:**
   - Login to admin panel (http://localhost:10645)
   - Navigate to Settings → General
   - Should see platform settings loaded

## 📚 Additional Resources

- [Full Documentation](https://docs.virtocommerce.org)
- [Developer Guide](https://docs.virtocommerce.org/platform/developer-guide/)
- [Module Development](https://docs.virtocommerce.org/platform/developer-guide/create-new-module/)
- [Frontend Documentation](https://docs.virtocommerce.org/storefront/)

## 🆘 Getting Help

If you encounter issues not covered here:

1. **Check GitHub Issues**: [Platform Issues](https://github.com/VirtoCommerce/vc-platform/issues)
2. **Community Forum**: [virtocommerce.org](https://www.virtocommerce.org)
3. **Documentation**: [docs.virtocommerce.org](https://docs.virtocommerce.org)
4. **Create New Issue**: Include error messages, OS version, and steps to reproduce

## 🤖 **MCP Server Module Installation (Optional)**

The [VirtoCommerce MCP Server module](https://github.com/VirtoCommerce/vc-module-mcp-server) enables AI agents like Claude Desktop to interact with VirtoCommerce APIs.

### **Prerequisites for MCP Server**
- VirtoCommerce Platform running successfully
- Node.js 18+ (for mcp-remote proxy)
- Claude Desktop (optional, for testing)

### **Installation Steps**

1. **Clone and build the MCP Server module:**
```bash
# In D:\work directory
git clone https://github.com/VirtoCommerce/vc-module-mcp-server.git
cd vc-module-mcp-server
dotnet build VirtoCommerce.McpServer.sln
```

2. **Deploy to VirtoCommerce:**
```bash
# Create module directory
New-Item -ItemType Directory -Path "D:\work\vc-platform\modules\VirtoCommerce.McpServer" -Force

# Copy built files
Copy-Item "src\VirtoCommerce.McpServer.Web\bin\Debug\net8.0\*" "D:\work\vc-platform\modules\VirtoCommerce.McpServer" -Recurse -Force

# Copy essential module files
Copy-Item "src\VirtoCommerce.McpServer.Web\module.manifest" "D:\work\vc-platform\modules\VirtoCommerce.McpServer\" -Force
New-Item -ItemType Directory -Path "D:\work\vc-platform\modules\VirtoCommerce.McpServer\Scripts" -Force
Copy-Item "src\VirtoCommerce.McpServer.Web\Scripts\*" "D:\work\vc-platform\modules\VirtoCommerce.McpServer\Scripts\" -Recurse -Force
```

3. **Install mcp-remote proxy:**
```bash
npm install -g mcp-remote
```

4. **Restart VirtoCommerce:**
```bash
# Stop current instance
Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue

# Start from correct directory
cd D:\work\vc-platform\src\VirtoCommerce.Platform.Web
dotnet run --environment Development
```

5. **Test MCP endpoints:**
```bash
# Test status endpoint
Invoke-WebRequest -Uri "http://localhost:10645/api/mcp/status" -Method GET

# Test tools listing
Invoke-WebRequest -Uri "http://localhost:10645/api/mcp/tools" -Method GET
```

### **Claude Desktop Configuration**

If using with Claude Desktop, update your `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "virtocommerce": {
      "command": "npx",
      "args": [
        "mcp-remote",
        "http://localhost:10645/api/mcp"
      ]
    }
  }
}
```

### **Available MCP Tools**

Once installed, the module provides:
- `search_customer_orders` - Search orders by customer, email, status, etc.
- Additional tools as defined in the module

### **Troubleshooting MCP Module**

**Issue**: MCP endpoints return 404
```bash
# Check if module is loaded
Get-ChildItem "D:\work\vc-platform\modules\VirtoCommerce.McpServer"
# Should show module.manifest and DLL files

# Check VirtoCommerce logs for module loading errors
# Look for "VirtoCommerce.McpServer" in startup logs
```

**Issue**: Module not recognized
- Ensure `module.manifest` file is present
- Verify module dependencies in manifest
- Check VirtoCommerce platform version compatibility

**Issue**: mcp-remote connection fails
```bash
# Verify mcp-remote is installed
npm list -g mcp-remote

# Test direct endpoint access
curl http://localhost:10645/api/mcp/status
```
