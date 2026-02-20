import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

type TenantState = { customerId: "acme-123" | "beta-456" };

const initialState: TenantState = { customerId: "acme-123" };

const tenantSlice = createSlice({
  name: "tenant",
  initialState,
  reducers: {
    setCustomerId(state, action: PayloadAction<TenantState["customerId"]>) {
      state.customerId = action.payload;
    },
  },
});

export const { setCustomerId } = tenantSlice.actions;
export default tenantSlice.reducer;
