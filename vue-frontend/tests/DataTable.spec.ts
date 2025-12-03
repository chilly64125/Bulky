import "./setup";
import { describe, it, expect, vi } from "vitest";
import { mount } from "@vue/test-utils";
import DataTable from "../src/components/global/DataTable.vue";

describe("DataTable", () => {
  const columns = [{ key: "name", label: "Name" }];
  const rows = [{ name: "Alice" }, { name: "Bob" }];

  it("renders rows and columns", () => {
    const wrapper = mount(DataTable, { props: { columns, rows } });
    expect(wrapper.text()).toContain("Alice");
    expect(wrapper.findAll("tbody tr").length).toBe(2);
  });

  it("shows action buttons when handlers provided", async () => {
    const onEdit = vi.fn();
    const onDelete = vi.fn();
    const wrapper = mount(DataTable, {
      props: { columns, rows, onEdit, onDelete },
    });
    const editButtons = wrapper.findAll("button.btn-warning");
    const deleteButtons = wrapper.findAll("button.btn-danger");
    expect(editButtons.length).toBe(2);
    expect(deleteButtons.length).toBe(2);
    await editButtons[0].trigger("click");
    expect(onEdit).toHaveBeenCalled();
  });
});
