export default function StatBox({ title, value, color }: { title: string; value: string; color?: string }) {
  const bgClass = color ?? "bg-white";
  return (
    <div className={`${bgClass} border rounded-xl p-3 min-w-[160px]`}>
      <div className="text-xs text-slate-600">{title}</div>
      <div className="text-xl font-semibold mt-1">{value}</div>
    </div>
  );
}
