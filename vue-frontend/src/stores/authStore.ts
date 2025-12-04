import { defineStore } from "pinia";
import { ref, computed } from "vue";
import type { User, AuthToken } from "@/types";
import { authService } from "@/services/authService";
import type { LoginRequest, RegisterRequest } from "@/types";

export const useAuthStore = defineStore("auth", () => {
  const user = ref<User | null>(null);
  const token = ref<AuthToken | null>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const isAuthenticated = computed(() => !!user.value && !!token.value);
  const hasRole = (role: string) => {
    const has = (user.value?.roles ?? []).includes(role);
    console.log(
      `🔐 hasRole('${role}'): ${has}, user.roles: ${JSON.stringify(user.value?.roles)}`
    );
    return has;
  };
  const isAdmin = computed(() => hasRole("Admin"));
  const isCustomer = computed(() => hasRole("Customer"));
  const isGuest = computed(() => hasRole("Guest"));

  const login = async (credentials: LoginRequest) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await authService.login(credentials);
      const { user: userData, token: tokenData } = response.data.data!;

      // normalize backend user shape to frontend `User` interface
      const rawUser: any = userData;
      console.log("🔍 Login response user data:", rawUser);
      console.log("🔍 Roles received:", rawUser.roles);

      const normalizedUser: User = {
        id: (rawUser.userId ?? rawUser.id ?? "") as string,
        username: (rawUser.username ??
          rawUser.name ??
          rawUser.email ??
          "") as string,
        email: rawUser.email ?? "",
        firstName: (rawUser.firstName ?? "") as string,
        lastName: (rawUser.lastName ?? "") as string,
        roles: (rawUser.roles ?? []) as string[],
        isActive: true,
      };

      console.log("✅ Normalized user:", normalizedUser);

      user.value = normalizedUser;
      token.value = tokenData;

      // Store in localStorage for persistence
      localStorage.setItem("user", JSON.stringify(normalizedUser));
      localStorage.setItem("accessToken", tokenData.accessToken);
      localStorage.setItem("refreshToken", tokenData.refreshToken);

      return true;
    } catch (err: any) {
      error.value = err.response?.data?.message || "Login failed";
      return false;
    } finally {
      loading.value = false;
    }
  };

  const register = async (data: RegisterRequest) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await authService.register(data);
      const { user: userData, token: tokenData } = response.data.data!;
      const rawUser2: any = userData;

      const normalizedUser: User = {
        id: (rawUser2.userId ?? rawUser2.id ?? "") as string,
        username: (rawUser2.username ??
          rawUser2.name ??
          rawUser2.email ??
          "") as string,
        email: rawUser2.email ?? "",
        firstName: (rawUser2.firstName ?? "") as string,
        lastName: (rawUser2.lastName ?? "") as string,
        roles: (rawUser2.roles ?? []) as string[],
        isActive: true,
      };

      user.value = normalizedUser;
      token.value = tokenData;

      localStorage.setItem("user", JSON.stringify(normalizedUser));
      localStorage.setItem("accessToken", tokenData.accessToken);
      localStorage.setItem("refreshToken", tokenData.refreshToken);

      return true;
    } catch (err: any) {
      error.value = err.response?.data?.message || "Registration failed";
      return false;
    } finally {
      loading.value = false;
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
    } catch (err) {
      console.error("Logout error:", err);
    } finally {
      token.value = null;
      localStorage.removeItem("user");
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
      // Set guest user after logout
      user.value = {
        id: "guest",
        username: "Guest",
        email: "guest@chen.local",
        firstName: "Guest",
        lastName: "User",
        roles: ["Guest"],
        isActive: true,
      };
    }
  };

  const initializeAuth = () => {
    const storedUser = localStorage.getItem("user");
    const accessToken = localStorage.getItem("accessToken");
    const refreshToken = localStorage.getItem("refreshToken");

    if (storedUser && accessToken) {
      user.value = JSON.parse(storedUser);
      token.value = {
        accessToken,
        refreshToken: refreshToken || "",
        expiresIn: 3600,
      };
    } else {
      // Set guest user if not authenticated
      user.value = {
        id: "guest",
        username: "Guest",
        email: "guest@chen.local",
        firstName: "Guest",
        lastName: "User",
        roles: ["Guest"],
        isActive: true,
      };
    }
  };

  const clearError = () => {
    error.value = null;
  };

  return {
    user,
    token,
    loading,
    error,
    isAuthenticated,
    isAdmin,
    isCustomer,
    isGuest,
    hasRole,
    login,
    register,
    logout,
    initializeAuth,
    clearError,
  };
});
