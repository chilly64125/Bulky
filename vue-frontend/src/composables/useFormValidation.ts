import { reactive } from "vue";
import * as yup from "yup";

export function useFormValidation<T extends Record<string, any>>(
  initialValues: T,
  schema: yup.ObjectSchema<any>
) {
  const values = reactive<T>({ ...initialValues });
  const errors = reactive<Partial<T>>({});
  const touched = reactive<Partial<Record<keyof T, boolean>>>({});

  async function validate() {
    try {
      await schema.validate(values, { abortEarly: false });
      Object.keys(errors).forEach((k) => delete (errors as any)[k]);
      return true;
    } catch (err: any) {
      Object.keys(errors).forEach((k) => delete (errors as any)[k]);
      if (err.inner && err.inner.length) {
        for (const e of err.inner) {
          if (e.path) (errors as any)[e.path] = e.message;
        }
      }
      return false;
    }
  }

  function markTouched(field: keyof T) {
    touched[field] = true;
  }

  function resetForm() {
    Object.assign(values, initialValues);
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    Object.keys(touched).forEach((k) => delete (touched as any)[k]);
  }

  function setFieldValue<K extends keyof T>(field: K, value: T[K]) {
    values[field] = value;
  }

  return {
    values,
    errors,
    touched,
    validate,
    markTouched,
    resetForm,
    setFieldValue,
  };
}
