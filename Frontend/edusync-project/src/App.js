import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Navbar from './components/layout/Navbar';
import Login from './components/auth/Login';
import Register from './components/auth/Register';
import Courses from './components/courses/Courses';
import CourseDetail from './components/courses/CourseDetail';
import Assessments from './components/assessments/Assessments';
import CreateAssessment from './components/assessments/CreateAssessment';
import EditAssessment from './components/assessments/EditAssessment';
import QuizV2 from './components/assessments/QuizV2';
import ProtectedRoute from './components/auth/ProtectedRoute';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <div className="App">
      <Navbar />
      <Routes>
        {/* Public routes */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        
        {/* Protected routes */}
        <Route path="/" element={
          <ProtectedRoute>
            <Courses />
          </ProtectedRoute>
        } />
        <Route path="/courses" element={
          <ProtectedRoute>
            <Courses />
          </ProtectedRoute>
        } />
        <Route path="/courses/:id" element={
          <ProtectedRoute>
            <CourseDetail />
          </ProtectedRoute>
        } />
        <Route path="/assessments/course/:courseId" element={
          <ProtectedRoute>
            <Assessments />
          </ProtectedRoute>
        } />
        <Route path="/assessments/course/:courseId/create" element={
          <ProtectedRoute>
            <CreateAssessment />
          </ProtectedRoute>
        } />
        <Route path="/assessments/course/:courseId/edit/:assessmentId" element={
          <ProtectedRoute>
            <EditAssessment />
          </ProtectedRoute>
        } />
        <Route path="/quiz/:assessmentId" element={
          <ProtectedRoute>
            <QuizV2 />
          </ProtectedRoute>
        } />
        
        {/* Catch all route - redirect to login */}
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </div>
  );
}

export default App;
