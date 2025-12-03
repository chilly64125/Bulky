import "./setup";
import { describe, it, expect } from "vitest";
import { mount } from "@vue/test-utils";
import LoadingSpinner from "../src/components/global/LoadingSpinner.vue";

describe("LoadingSpinner", () => {
  it("renders default loading spinner", () => {
    const wrapper = mount(LoadingSpinner);
    expect(wrapper.text()).toContain("Loading...");
  });

  it("shows message when provided", () => {
    const wrapper = mount(LoadingSpinner, {
      props: { message: "Please wait" },
    });
    expect(wrapper.text()).toContain("Please wait");
  });
});
