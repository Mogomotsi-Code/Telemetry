import { useState } from "react";
import { skipToken } from "@reduxjs/toolkit/query";
import {  useAppSelector } from "../hooks/useAppSelector";
import { useAppDispatch } from "../hooks/useDispatch";
import { setCustomerId } from "../store/tenantSlice";
import {
  useGetDevicesQuery,
  useGetTelemetryLast24hQuery,
  useGetInsights24hQuery,
} from "../services/telemetryApi";
import StatBox from "../components/StatBox";

const customers = ["acme-123", "beta-456"] as const;

export default function App() {
  const dispatch = useAppDispatch();
  const customerId = useAppSelector((s) => s.tenant.customerId);
  const [deviceId, setDeviceId] = useState("");
  const devicesQ = useGetDevicesQuery({ customerId });
  const insightsQ = useGetInsights24hQuery(
    deviceId ? { customerId, deviceId, windowHours: 24 } : skipToken
  );
  const telemetryQ = useGetTelemetryLast24hQuery(
    deviceId ? { customerId, deviceId } : skipToken
  );

  return (
    <div style={{ fontFamily: "system-ui", padding: 16, maxWidth: 1000, margin: "0 auto" }}>
      <h2>Telemetry Explorer</h2>

      <div style={{ display: "flex", gap: 16, alignItems: "center", marginBottom: 16 }}>
        <label>
          Customer:&nbsp;
          <select
            value={customerId}
            onChange={(e) => {
              dispatch(setCustomerId(e.target.value as any));
              setDeviceId("");
            }}
          >
            {customers.map((c) => (
              <option key={c} value={c}>{c}</option>
            ))}
          </select>
        </label>

        <label>
          Device:&nbsp;
          <select
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
        </label>

        <button onClick={() => devicesQ.refetch()} disabled={devicesQ.isFetching}>
          Refresh devices
        </button>
      </div>

      {(devicesQ.error || telemetryQ.error || insightsQ.error) && (
        <div style={{ background: "#fee", padding: 12, borderRadius: 8 }}>
          {String(devicesQ.error ?? telemetryQ.error ?? insightsQ.error)}
        </div>
      )}

      {insightsQ.data && (
        <div style={{ display: "flex", gap: 16, marginBottom: 16, flexWrap: "wrap" }}>
                  <StatBox title="Latest" value={insightsQ.data.payload?.latest ? `${insightsQ.data.payload?.latest[0].value} ${insightsQ.data.payload?.latest[0].unit}` : "—"} />
                  <StatBox title="Min" value={insightsQ.data.payload?.stats ? insightsQ.data.payload?.stats.min.toFixed(2) : "—"} />
                  <StatBox title="Avg" value={insightsQ.data.payload?.stats ? insightsQ.data.payload?.stats.avg.toFixed(2) : "—"} />
                  <StatBox title="Max" value={insightsQ.data.payload?.stats ? insightsQ.data.payload?.stats.max.toFixed(2) : "—"} />
        </div>
      )}

      <h3>Last 24 hours</h3>
      <table width="100%" cellPadding={8} style={{ borderCollapse: "collapse" }}>
        <thead>
          <tr style={{ textAlign: "left", borderBottom: "1px solid #ddd" }}>
            <th>Recorded At (UTC)</th>
            <th>Event</th>
            <th>Type</th>
            <th>Value</th>
          </tr>
        </thead>
        <tbody>
          {(telemetryQ.data?.payload ?? []).map((e) => (
            <tr key={e.eventId} style={{ borderBottom: "1px solid #f0f0f0" }}>
              <td>{new Date(e.recordedAt).toISOString()}</td>
              <td>{e.eventId}</td>
              <td>{e.type}</td>
              <td>{e.value} {e.unit}</td>
            </tr>
          ))}
          {(telemetryQ.data?.payload?.length ?? 0) === 0 && (
            <tr><td colSpan={4} style={{ opacity: 0.7 }}>
              {deviceId ? (telemetryQ.isFetching ? "Loading..." : "No data in window.") : "Select a device."}
            </td></tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

