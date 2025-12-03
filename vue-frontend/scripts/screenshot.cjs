const { chromium } = require("playwright");

(async () => {
  const browser = await chromium.launch({ headless: true });
  const page = await browser.newPage({
    viewport: { width: 1280, height: 800 },
  });
  const url = process.argv[2] || "http://localhost:5173/";
  console.log("Opening", url);
  await page.goto(url, { waitUntil: "networkidle" });
  const out = process.argv[3] || "../frontend-homepage.png";
  await page.screenshot({ path: out, fullPage: true });
  console.log("Saved screenshot to", out);
  await browser.close();
})();
