import { configureStore } from "@reduxjs/toolkit";
import tenantReducer from "./tenantSlice";
import { telemetryApi } from "../services/telemetryApi";

export const store = configureStore({
  reducer: {
    tenant: tenantReducer,
    [telemetryApi.reducerPath]: telemetryApi.reducer,
  },
  middleware: (getDefault) => getDefault().concat(telemetryApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
