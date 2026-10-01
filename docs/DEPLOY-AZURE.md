# Deploying to Azure App Service

This guide gets the calculator live on the web so you can use it from any browser or phone.

There are **two parts**: a one-time setup in the Azure Portal (about 10 minutes), and connecting it to GitHub so every push auto-deploys.

---

## Part 1 — Create the App Service (one time)

You need a free [Azure account](https://azure.microsoft.com/free/). No credit card charge for the free tier.

### Option A: In the browser (recommended)

1. Go to the [Azure Portal](https://portal.azure.com) and sign in.
2. Click **Create a resource** → search for **Web App** → **Create**.
3. Fill in the basics:
   - **Subscription**: your free trial / pay-as-you-go subscription
   - **Resource group**: create new, name it `print-calculator`
   - **Name**: e.g. `print-cost-calculator-<your-name>` — this becomes your URL: `https://<name>.azurewebsites.net`
   - **Publish**: **Code**
   - **Runtime stack**: **.NET 8 (LTS)**
   - **Operating System**: **Linux**
   - **Region**: North Europe (closest to Sweden)
   - **Pricing plan**: for always-on hosting choose **B1 (Basic)** (~50-75 SEK/month). The **F1 (Free)** tier works too but the site sleeps after 20 idle minutes and takes ~10 seconds to wake up.
4. Click **Review + create** → **Create** and wait for deployment (~1 minute).

### Option B: With the Azure CLI

```bash
az login
az group create --name print-calculator --location northeurope
az appservice plan create --name print-calculator-plan --resource-group print-calculator --sku B1 --is-linux
az webapp create --name print-cost-calculator-YOURNAME --resource-group print-calculator \
  --plan print-calculator-plan --runtime "DOTNETCORE|8.0"
```

---

## Part 2 — Connect GitHub auto-deploy

The repository contains `.github/workflows/azure-deploy.yml`, which builds, tests, and deploys the app on every push to `main`. You only need to give it permission once:

1. In the Azure Portal, open your new **Web App**.
2. In the left menu, open **Deployment Center** (or **Deployment** → **Deployment Center**).
3. Go to the **Publish profile** — click **Manage publish profile** → **Download publish profile**. This downloads a small `.publishsettings` file. **It contains secrets — treat it like a password.**
4. Go to your GitHub repo → **Settings** → **Secrets and variables** → **Actions** → **New repository secret**:
   - **Name**: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - **Value**: the entire contents of the downloaded file, pasted as-is.
5. Open `.github/workflows/azure-deploy.yml` and set `AZURE_WEBAPP_NAME` to your exact App Service name (step 3 above).
6. Push any commit to `main` — the **Actions** tab will show the workflow building and deploying.

When it finishes, your app is live at:

```
https://<your-app-name>.azurewebsites.net
```

Open that URL on your phone and (optionally) choose **Add to Home Screen** — it installs like a native app.

---

## Troubleshooting

| Problem | Fix |
|---|---|
| Workflow fails at the deploy step with a 401/403 error | The publish-profile secret is wrong or expired — re-download it and update `AZURE_WEBAPP_PUBLISH_PROFILE`. |
| Workflow fails at `dotnet test` | The tests caught a real bug — check the Actions log; the deploy is blocked until it's fixed. |
| First phone visit is slow (F1 tier) | The free tier sleeps after 20 idle minutes. Upgrade to B1 for always-on. |
| `AZURE_WEBAPP_NAME` mismatch | The name in the workflow must exactly match the App Service name in Azure. |

---

## Cost summary

- **F1 Free**: SEK 0/month, but sleeps after 20 idle minutes
- **B1 Basic**: ~50–75 SEK/month, always on

## Removing it later

```bash
az group delete --name print-calculator
```

Deletes the whole resource group, app, and plan — no further charges.
