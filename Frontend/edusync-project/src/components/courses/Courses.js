import React, { useState, useEffect, useCallback } from 'react';
import { useAuth } from '../../context/AuthContext';
import axios from '../../utils/axiosConfig';
import { Link } from 'react-router-dom';

function Courses() {
  const { user } = useAuth();
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editingCourse, setEditingCourse] = useState(null);

  const [newCourse, setNewCourse] = useState({
    title: '',
    description: '',
    mediaUrl: ''
  });

  const fetchCourses = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await axios.get('/Courses');
      const coursesData = Array.isArray(response.data) ? response.data : [response.data];
      setCourses(coursesData);
    } catch (err) {
      console.error('Error fetching courses:', err);
      setError('Failed to fetch courses. Please try again later.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    if (user) {
      fetchCourses();
    } else {
      setLoading(false);
    }
  }, [user, fetchCourses]);

  const handleAddCourse = async (e) => {
    e.preventDefault();
    try {
      const courseData = { ...newCourse };
      await axios.post('/Courses', courseData);
      setNewCourse({ title: '', description: '', mediaUrl: '' });
      fetchCourses();
    } catch (err) {
      console.error('Error adding course:', err);
      setError('Failed to add course.');
    }
  };

  const handleDeleteCourse = async (id) => {
    if (!window.confirm('Are you sure you want to delete this course?')) return;
    try {
      await axios.delete(`/Courses/${id}`);
      fetchCourses();
    } catch (err) {
      console.error('Error deleting course:', err);
      setError('Failed to delete course.');
    }
  };

  const handleUpdateCourse = async (e) => {
    e.preventDefault();
    try {
      await axios.put(`/Courses/${editingCourse.courseId}`, editingCourse);
      setEditingCourse(null);
      fetchCourses();
    } catch (err) {
      console.error('Error updating course:', err);
      setError('Failed to update course.');
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewCourse(prev => ({ ...prev, [name]: value }));
  };

  const handleEditInputChange = (e) => {
    const { name, value } = e.target;
    setEditingCourse(prev => ({ ...prev, [name]: value }));
  };

  if (loading) return <div className="container mt-4">Loading courses...</div>;

  if (error) {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger">{error}</div>
        <button onClick={fetchCourses} className="btn btn-primary">Retry</button>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Available Courses</h2>

      {user?.role === 'Instructor' && (
        <div className="card mb-4">
          <div className="card-body">
            <h3 className="card-title">Add New Course</h3>
            <form onSubmit={handleAddCourse}>
              <div className="mb-3">
                <label htmlFor="title" className="form-label">Title</label>
                <input type="text" className="form-control" name="title" value={newCourse.title} onChange={handleInputChange} required />
              </div>
              <div className="mb-3">
                <label htmlFor="description" className="form-label">Description</label>
                <textarea className="form-control" name="description" value={newCourse.description} onChange={handleInputChange} required />
              </div>
              <div className="mb-3">
                <label htmlFor="mediaUrl" className="form-label">Media URL</label>
                <input type="url" className="form-control" name="mediaUrl" value={newCourse.mediaUrl} onChange={handleInputChange} required />
              </div>
              <button type="submit" className="btn btn-primary">Add Course</button>
            </form>
          </div>
        </div>
      )}

      {/* Edit Course Form */}
      {user?.role === 'Instructor' && editingCourse && (
        <div className="card mb-4">
          <div className="card-body">
            <h3>Edit Course</h3>
            <form onSubmit={handleUpdateCourse}>
              <div className="mb-3">
                <label className="form-label">Title</label>
                <input className="form-control" name="title" value={editingCourse.title} onChange={handleEditInputChange} required />
              </div>
              <div className="mb-3">
                <label className="form-label">Description</label>
                <textarea className="form-control" name="description" value={editingCourse.description} onChange={handleEditInputChange} required />
              </div>
              <div className="mb-3">
                <label className="form-label">Media URL</label>
                <input className="form-control" name="mediaUrl" value={editingCourse.mediaUrl} onChange={handleEditInputChange} required />
              </div>
              <button className="btn btn-success" type="submit">Save</button>
              <button className="btn btn-secondary ms-2" onClick={() => setEditingCourse(null)}>Cancel</button>
            </form>
          </div>
        </div>
      )}

      {/* Course List */}
      <div className="row">
        {courses.length === 0 ? (
          <div className="col-12">
            <div className="alert alert-info">No courses available.</div>
          </div>
        ) : (
          courses.map(course => (
            <div key={course.courseId} className="col-md-4 mb-4">
              <div className="card h-100">
                <div className="card-body">
                  <h5 className="card-title">{course.title}</h5>
                  <p className="card-text">{course.description}</p>
                  <Link to={`/courses/${course.courseId}`} className="btn btn-primary">View Details</Link>

                  {user?.role === 'Instructor' && (
                    <div className="mt-3">
                      <button className="btn btn-warning me-2" onClick={() => setEditingCourse(course)}>Edit</button>
                      <button className="btn btn-danger" onClick={() => handleDeleteCourse(course.courseId)}>Delete</button>
                    </div>
                  )}
                </div>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}

export default Courses;
