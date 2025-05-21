import React, { useEffect, useState } from 'react';
import { Bar } from 'react-chartjs-2';

export default function Dashboard() {
  const [chartData, setChartData] = useState({ labels: [], datasets: [] });

  useEffect(() => {
    const socket = new WebSocket('ws://localhost:5000/analytics');
    socket.onmessage = (e) => {
      const data = JSON.parse(e.data);
      setChartData({
        labels: data.labels,
        datasets: [{
          label: 'Scores',
          data: data.scores,
          backgroundColor: 'rgba(54, 162, 235, 0.6)',
        }]
      });
    };
    return () => socket.close();
  }, []);

  return (
    <div className="container mt-4">
      <h2>Live Quiz Analytics</h2>
      <Bar data={chartData} />
    </div>
  );
}
