import "./setup";
import { describe, it, expect, vi, beforeEach } from "vitest";
import { setActivePinia, createPinia } from "pinia";
import { useAuthStore } from "../src/stores/authStore";

vi.mock("../src/services/authService", () => {
  return {
    authService: {
      login: vi.fn(),
      register: vi.fn(),
      logout: vi.fn(),
    },
  };
});

import { authService } from "../src/services/authService";

describe("authStore", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
    localStorage.clear();
    (authService.login as any).mockReset();
    (authService.register as any).mockReset();
  });

  it("logs in successfully and stores tokens", async () => {
    const fakeResponse = {
      data: {
        data: {
          user: { id: "u1", username: "test", roles: ["Customer"] },
          token: {
            accessToken: "a.token",
            refreshToken: "r.token",
            expiresIn: 3600,
          },
        },
      },
    };

    (authService.login as any).mockResolvedValue(fakeResponse);

    const store = useAuthStore();
    const result = await store.login({ email: "a@b.com", password: "secret" });

    expect(result).toBe(true);
    expect(store.user).not.toBeNull();
    expect(localStorage.getItem("accessToken")).toBe("a.token");
    expect(localStorage.getItem("refreshToken")).toBe("r.token");
  });

  it("handles login failure gracefully", async () => {
    const fakeError = {
      response: { data: { message: "Invalid credentials" } },
    };
    (authService.login as any).mockRejectedValue(fakeError);

    const store = useAuthStore();
    const result = await store.login({ email: "a@b.com", password: "bad" });

    expect(result).toBe(false);
    expect(store.error).toBe("Invalid credentials");
    expect(localStorage.getItem("accessToken")).toBeNull();
  });
});
