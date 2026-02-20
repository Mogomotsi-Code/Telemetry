export type Insights = {
  windowHours: number;
  latest: null | { recordedAt: string; value: number; unit: string; type: string; eventId: string };
  stats: null | { min: number; avg: number; max: number };
};