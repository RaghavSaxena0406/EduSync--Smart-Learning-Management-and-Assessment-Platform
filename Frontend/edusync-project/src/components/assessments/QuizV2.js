import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from '../../utils/axiosConfig';
import { useAuth } from '../../context/AuthContext';

function QuizV2() {
  const { assessmentId } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [assessment, setAssessment] = useState(null);
  const [questions, setQuestions] = useState([]);
  const [selectedAnswers, setSelectedAnswers] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [submitted, setSubmitted] = useState(false);
  const [score, setScore] = useState(null);

  useEffect(() => {
    const fetchAssessment = async () => {
      try {
        setLoading(true);
        const response = await axios.get(`/Assessments/${assessmentId}`);
        const assessmentData = response.data;
        
        // Parse questions if they are stored as a JSON string
        const parsedQuestions = typeof assessmentData.questions === 'string' 
          ? JSON.parse(assessmentData.questions) 
          : assessmentData.questions;

        setAssessment(assessmentData);
        setQuestions(parsedQuestions || []);
        
        // Initialize selectedAnswers with empty values
        const initialAnswers = {};
        parsedQuestions?.forEach(q => {
          initialAnswers[q.questionId] = null;
        });
        setSelectedAnswers(initialAnswers);
      } catch (err) {
        console.error('Error fetching assessment:', err);
        setError('Failed to load assessment. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchAssessment();
  }, [assessmentId]);

  const handleOptionSelect = (questionId, option) => {
    setSelectedAnswers(prev => ({
      ...prev,
      [questionId]: option
    }));
  };

  const calculateScore = () => {
    let totalScore = 0;
    questions.forEach(question => {
      if (selectedAnswers[question.questionId] === question.correctAnswer) {
        totalScore += question.points || 1;
      }
    });
    return totalScore;
  };

  const handleSubmit = async () => {
    if (!window.confirm('Are you sure you want to submit your answers?')) {
      return;
    }

    try {
      const finalScore = calculateScore();
      setScore(finalScore);
      setSubmitted(true);

      // Submit the assessment result to the backend
      await axios.post('/AssessmentResults', {
        assessmentId: assessmentId,
        studentId: user.id,
        score: finalScore,
        maxScore: assessment.maxScore,
        answers: JSON.stringify(Object.entries(selectedAnswers).map(([questionId, answer]) => ({
          questionId,
          answer
        })))
      });

    } catch (err) {
      console.error('Error submitting assessment:', err);
      setError('Failed to submit assessment. Please try again later.');
    }
  };

  if (loading) {
    return <div className="container mt-4">Loading assessment...</div>;
  }

  if (error) {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger">{error}</div>
        <button onClick={() => navigate(-1)} className="btn btn-primary">Go Back</button>
      </div>
    );
  }

  if (submitted) {
    return (
      <div className="container mt-4">
        <div className="card">
          <div className="card-body">
            <h2 className="card-title">Assessment Completed!</h2>
            <p className="card-text">Your score: {score} out of {assessment.maxScore}</p>
            <button onClick={() => navigate(-1)} className="btn btn-primary">Back to Assessments</button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2>{assessment?.title}</h2>
      <p className="mb-4">Total Points: {assessment?.maxScore}</p>

      <form onSubmit={(e) => { e.preventDefault(); handleSubmit(); }}>
        {questions.map((question, index) => (
          <div key={question.questionId} className="card mb-4">
            <div className="card-body">
              <h5 className="card-title">Question {index + 1}</h5>
              <p className="card-text">{question.text}</p>
              <div className="form-group">
                {question.options?.map((option, optIndex) => (
                  <div key={`${question.questionId}-${optIndex}`} className="form-check">
                    <input
                      type="radio"
                      className="form-check-input"
                      id={`q${question.questionId}o${optIndex}`}
                      name={`question_${question.questionId}`}
                      value={option}
                      checked={selectedAnswers[question.questionId] === option}
                      onChange={() => handleOptionSelect(question.questionId, option)}
                      required
                    />
                    <label className="form-check-label" htmlFor={`q${question.questionId}o${optIndex}`}>
                      {option}
                    </label>
                  </div>
                ))}
              </div>
            </div>
          </div>
        ))}

        <div className="d-flex gap-2 mb-4">
          <button type="submit" className="btn btn-primary">Submit Assessment</button>
          <button type="button" onClick={() => navigate(-1)} className="btn btn-secondary">Cancel</button>
        </div>
      </form>
    </div>
  );
}

export default QuizV2; 