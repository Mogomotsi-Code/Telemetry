export type TelemetryIn = {
  customerId?: string | null;
  deviceId: string;
  eventId: string;
  recordedAt: string; 
  type?: string | null;
  value: number;
  unit?: string | null;
};