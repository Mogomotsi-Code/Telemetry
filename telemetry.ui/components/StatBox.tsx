export default function StatBox({ title, value }: { title: string; value: string }) {
  return (
    <div style={{ border: "1px solid #ddd", borderRadius: 12, padding: 12, minWidth: 160 }}>
      <div style={{ fontSize: 12, opacity: 0.7 }}>{title}</div>
      <div style={{ fontSize: 20, fontWeight: 700 }}>{value}</div>
    </div>
  );
}