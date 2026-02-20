import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { RootState } from "../store/store";
import type { Device } from "../types/Device";
import type { Insights } from "../types/Insights";
import type { TelemetryEvent } from "../types/TelemetryEvent";
import type { TelemetryIn } from "../types/TelemetryIn";
import type { Response } from "../types/Response";

const API_BASE = "https://localhost:7284";

export const telemetryApi = createApi({
  reducerPath: "telemetryApi",
  baseQuery: fetchBaseQuery({
    baseUrl: API_BASE,
    prepareHeaders: (headers, { getState }) => {
      // Pull tenant from redux
      const state = getState() as RootState;
      const customerId = state.tenant.customerId;
      headers.set("X-Customer-Id", customerId);
      return headers;
    },
  }),
  tagTypes: ["Devices", "Telemetry", "Insights"],
  endpoints: (builder) => ({
    // IMPORTANT: include customerId in args so cache is tenant-safe
      getDevices: builder.query<Response<Device[]>, void>({
      query: () => `/api/devices`,
      providesTags: (result) =>
        result && result.payload
          ? [{ type: "Devices", id: "LIST" }, ...result.payload.map((d) => ({ type: "Devices" as const, id: d.deviceId }))]
          : [{ type: "Devices", id: "LIST" }],
    }),

      getTelemetryLast24h: builder.query<Response<TelemetryEvent[]>, { customerId: string; deviceId: string }>({
      query: ({ deviceId }) => {
        const since = new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString();
        const until = new Date().toISOString();
        return `/api/telemetry/${encodeURIComponent(deviceId)}/telemetry?since=${encodeURIComponent(
          since
        )}&until=${encodeURIComponent(until)}`;
      },
      providesTags: (_res, _err, arg) => [{ type: "Telemetry", id: `${arg.customerId}:${arg.deviceId}` }],
    }),

      getInsights24h: builder.query<Response<Insights>, { customerId: string; deviceId: string; windowHours?: number }>({
      query: ({ deviceId, windowHours = 24 }) =>
        `/api/telemetry/${encodeURIComponent(deviceId)}/insights?windowHours=${windowHours}`,
      providesTags: (_res, _err, arg) => [{ type: "Insights", id: `${arg.customerId}:${arg.deviceId}` }],
    }),

      ingestTelemetry: builder.mutation<Response<{ inserted: boolean; duplicate?: boolean }>, { customerId: string; body: TelemetryIn }>(
      {
        query: ({ body }) => ({
          url: `/api/telemetry`,
          method: "POST",
          body,
        }),
        invalidatesTags: (_res, _err, arg) => [
          { type: "Telemetry", id: `${arg.customerId}:${arg.body.deviceId}` },
          { type: "Insights", id: `${arg.customerId}:${arg.body.deviceId}` },
        ],
      }
    ),
  }),
});

export const {
  useGetDevicesQuery,
  useGetTelemetryLast24hQuery,
  useGetInsights24hQuery,
  useIngestTelemetryMutation,
} = telemetryApi;