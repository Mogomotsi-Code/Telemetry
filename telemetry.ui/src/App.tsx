import { useState } from "react";
import { skipToken } from "@reduxjs/toolkit/query";
import { useAppSelector } from "../hooks/useAppSelector";
import { useAppDispatch } from "../hooks/useDispatch";
import { setCustomerId } from "../store/tenantSlice";
import { useGetDevicesQuery, useGetTelemetryLast24hQuery, useGetInsights24hQuery } from "../services/telemetryApi";
import StatBox from "../components/StatBox";

const customers = ["acme-123", "beta-456"] as const;

export default function App() {
  const dispatch = useAppDispatch();
  const customerId = useAppSelector((s) => s.tenant.customerId);
  const [deviceId, setDeviceId] = useState("");
  const devicesQ = useGetDevicesQuery({ customerId });
  const insightsQ = useGetInsights24hQuery(deviceId ? { customerId, deviceId, windowHours: 24 } : skipToken);
  const telemetryQ = useGetTelemetryLast24hQuery(deviceId ? { customerId, deviceId } : skipToken);

  return (
    <div className="min-h-screen p-6">
      <div className="max-w-5xl mx-auto">
        <header className="flex items-center justify-between mb-6">
          <h1 className="text-2xl font-semibold">Telemetry Explorer</h1>
          <div>
            <button
              className="px-3 py-2 rounded bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-60"
              onClick={() => devicesQ.refetch()}
              disabled={devicesQ.isFetching}
            >
              {devicesQ.isFetching ? "Refreshing..." : "Refresh devices"}
            </button>
          </div>
        </header>

        <section className="bg-white rounded-lg shadow p-4 mb-6">
          <div className="flex gap-4 flex-wrap mb-4">
            <div className="flex flex-col min-w-[200px]">
              <label className="text-xs text-slate-600 mb-1">Customer</label>
              <select
                className="px-3 py-2 rounded border border-slate-200"
                value={customerId}
                onChange={(e) => {
                  dispatch(setCustomerId(e.target.value as any));
                  setDeviceId("");
                }}
              >
                {customers.map((c) => (
                  <option key={c} value={c}>
                    {c}
                  </option>
                ))}
              </select>
            </div>

            <div className="flex flex-col min-w-[200px]">
              <label className="text-xs text-slate-600 mb-1">Device</label>
              <select
                className="px-3 py-2 rounded border border-slate-200"
                value={deviceId}
                onChange={(e) => setDeviceId(e.target.value)}
                disabled={devicesQ.isLoading || !!devicesQ.error}
              >
                <option value="">-- select --</option>
                {(devicesQ.data?.payload ?? []).map((d) => (
                  <option key={d.deviceId} value={d.deviceId}>
                    {d.deviceId} — {d.label}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {(devicesQ.error || telemetryQ.error || insightsQ.error) && (
            <div className="bg-red-50 text-red-700 p-3 rounded">{String(devicesQ.error ?? telemetryQ.error ?? insightsQ.error)}</div>
          )}

          {insightsQ.data && (
            <div className="flex gap-3 flex-wrap mt-3">
              <StatBox color="bg-blue-50" title="Latest" value={insightsQ.data.payload?.latest ? `${insightsQ.data.payload?.latest.value} ${insightsQ.data.payload?.latest.unit}` : "—"} />
              <StatBox color="bg-green-50" title="Min" value={insightsQ.data.payload?.stats ? insightsQ.data.payload?.stats.min.toFixed(2) : "—"} />
              <StatBox color="bg-sky-50" title="Avg" value={insightsQ.data.payload?.stats ? insightsQ.data.payload?.stats.avg.toFixed(2) : "—"} />
              <StatBox color="bg-amber-50" title="Max" value={insightsQ.data.payload?.stats ? insightsQ.data.payload?.stats.max.toFixed(2) : "—"} />
            </div>
          )}
        </section>

        <section className="bg-white rounded-lg shadow p-4">
          <h3 className="text-lg mb-3">Last 24 hours</h3>
          <div className="overflow-x-auto">
            <table className="min-w-full table-auto text-sm">
              <thead>
                <tr className="text-left border-b">
                  <th className="py-2">Recorded At (UTC)</th>
                  <th className="py-2">Event</th>
                  <th className="py-2">Type</th>
                  <th className="py-2 text-right">Value</th>
                </tr>
              </thead>
              <tbody>
                {(telemetryQ.data?.payload ?? []).map((e) => (
                  <tr key={e.eventId} className="border-b even:bg-slate-50">
                    <td className="py-2">{new Date(e.recordedAt).toISOString()}</td>
                    <td className="py-2">{e.eventId}</td>
                    <td className="py-2">{e.type}</td>
                    <td className="py-2 text-right">{e.value} {e.unit}</td>
                  </tr>
                ))}

                {(telemetryQ.data?.payload?.length ?? 0) === 0 && (
                  <tr>
                    <td colSpan={4} className="py-8 text-center text-slate-500">
                      {deviceId ? (telemetryQ.isFetching ? "Loading..." : "No data in window.") : "Select a device."}
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </section>
      </div>
    </div>
  );
}
const theme = {
    primary: "#2563eb",
    success: "#16a34a",
    info: "#0ea5e9",
    warning: "#f59e0b",
    bg: "#f6f8fa",
};
const styles: Record<string, React.CSSProperties> = {
  page: { fontFamily: "Inter, system-ui, -apple-system, 'Segoe UI', Roboto, 'Helvetica Neue', Arial", padding: 20, maxWidth: 1100, margin: "0 auto", background: theme.bg },
  header: { display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 16 },
  headerActions: { display: "flex", gap: 8 },
  refreshButton: { padding: "8px 12px", borderRadius: 6, border: "1px solid transparent", background: theme.primary, color: "#fff", cursor: "pointer" },
  card: { background: "#fff", padding: 16, borderRadius: 8, boxShadow: "0 1px 4px rgba(0,0,0,0.06)", marginBottom: 16 },
  controls: { display: "flex", gap: 12, alignItems: "flex-end", flexWrap: "wrap" },
  controlItem: { display: "flex", flexDirection: "column", minWidth: 200 },
  label: { fontSize: 12, color: "#555", marginBottom: 6 },
  select: { padding: "8px 10px", borderRadius: 6, border: "1px solid #ddd", background: "#fff" },
  table: { width: "100%", borderCollapse: "collapse", minWidth: 600 },
  errorBox: { background: "#fff6f6", color: "#900", padding: 12, borderRadius: 6, border: "1px solid #f0caca", marginTop: 12 },
  statsRow: { display: "flex", gap: 12, marginTop: 12, flexWrap: "wrap" },
};



