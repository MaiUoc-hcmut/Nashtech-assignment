import { useState } from 'react';
import { BarChart as ReChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { SalesData, TimePeriod } from '../types/dashboardTypes';

interface SalesChartProps {
  data: {
    weekly: SalesData[];
    monthly: SalesData[];
    yearly: SalesData[];
  };
}

const SalesChart: React.FC<SalesChartProps> = ({ data }) => {
  const [period, setPeriod] = useState<TimePeriod>(TimePeriod.Weekly);

  const getChartData = (): SalesData[] => {
    switch (period) {
      case TimePeriod.Weekly:
        return data.weekly;
      case TimePeriod.Monthly:
        return data.monthly;
      case TimePeriod.Yearly:
        return data.yearly;
      default:
        return data.weekly;
    }
  };

  const getTitle = (): string => {
    switch (period) {
      case TimePeriod.Weekly:
        return 'Weekly Revenue';
      case TimePeriod.Monthly:
        return 'Monthly Revenue';
      case TimePeriod.Yearly:
        return 'Yearly Revenue';
      default:
        return 'Weekly Revenue';
    }
  };

  return (
    <div className="lg:col-span-2 bg-white rounded-lg shadow p-6">
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-xl font-semibold">{getTitle()}</h2>
        <select
          value={period}
          onChange={(e) => setPeriod(e.target.value as TimePeriod)}
          className="border border-gray-300 rounded-md px-3 py-1 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value={TimePeriod.Weekly}>Weekly</option>
          <option value={TimePeriod.Monthly}>Monthly</option>
          <option value={TimePeriod.Yearly}>Yearly</option>
        </select>
      </div>
      <div className="h-64">
        <ResponsiveContainer width="100%" height="100%">
          <ReChart data={getChartData()}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="revenue" fill="#4F46E5" />
          </ReChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default SalesChart;