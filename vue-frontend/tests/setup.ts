// Vitest setup: simple localStorage polyfill for Node environment
if (typeof (globalThis as any).localStorage === "undefined") {
  const store = new Map<string, string | null>();
  const localStoragePoly = {
    getItem(key: string) {
      return store.has(key) ? (store.get(key) ?? null) : null;
    },
    setItem(key: string, value: string) {
      store.set(key, String(value));
    },
    removeItem(key: string) {
      store.delete(key);
    },
    clear() {
      store.clear();
    },
  };
  (globalThis as any).localStorage = localStoragePoly;
}
