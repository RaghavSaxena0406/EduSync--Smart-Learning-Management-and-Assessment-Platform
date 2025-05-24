import React, { useEffect, useState } from 'react';

export default function Results() {
  const [results, setResults] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem('token');
    fetch(`${process.env.REACT_APP_API_URL}/results`, {
      headers: { Authorization: `Bearer ${token}` }
    }).then(res => res.json()).then(setResults);
  }, []);

  return (
    <div className="container mt-4">
      <h2>Assessment Results</h2>
      <table className="table table-striped">
        <thead>
          <tr>
            <th>Assessment</th>
            <th>Score</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          {results.map(r => (
            <tr key={r.resultId}>
              <td>{r.assessmentId}</td>
              <td>{r.score}</td>
              <td>{new Date(r.attemptDate).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
