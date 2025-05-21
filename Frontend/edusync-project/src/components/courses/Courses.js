import React, { useState, useEffect, useCallback } from 'react';
import { useAuth } from '../../context/AuthContext';
import axios from '../../utils/axiosConfig';
import { Link } from 'react-router-dom';

function Courses() {
  const { user } = useAuth();
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Form state for adding new course
  const [newCourse, setNewCourse] = useState({
    title: '',
    description: '',
    mediaUrl: ''
  });

  const fetchCourses = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      
      console.log('Fetching courses...');
      console.log('Current user:', user);
      console.log('Auth token:', localStorage.getItem('token'));
      
      const response = await axios.get('/Courses');
      console.log('Courses response:', response.data);
      
      if (!response.data) {
        throw new Error('No courses data received');
      }
      
      // Ensure we have an array of courses
      const coursesData = Array.isArray(response.data) ? response.data : [response.data];
      console.log('Processed courses data:', coursesData);
      
      setCourses(coursesData);
    } catch (err) {
      console.error('Error fetching courses:', err);
      console.error('Error details:', {
        message: err.message,
        response: err.response?.data,
        status: err.response?.status
      });
      
      if (err.response?.status === 401) {
        setError('Please log in to view courses.');
      } else if (err.message === 'Network Error') {
        setError('Unable to connect to the server. Please check your connection.');
      } else {
        setError(err.response?.data?.message || 'Failed to fetch courses. Please try again later.');
      }
    } finally {
      setLoading(false);
    }
  }, [user]);

  useEffect(() => {
    console.log('Courses component mounted');
    if (user) {
      fetchCourses();
    } else {
      console.log('No user found, waiting for auth...');
      setLoading(false);
    }
  }, [user, fetchCourses]);

  const handleAddCourse = async (e) => {
    e.preventDefault();
    try {
      const courseData = {
        ...newCourse,
        instructorId: user.id
      };

      console.log('Adding new course:', courseData);
      await axios.post('/Courses', courseData);
      setNewCourse({ title: '', description: '', mediaUrl: '' });
      fetchCourses(); // Refresh the courses list
    } catch (err) {
      console.error('Error adding course:', err);
      setError('Failed to add course. Please try again.');
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewCourse(prev => ({
      ...prev,
      [name]: value
    }));
  };

  if (loading) {
    return <div className="container mt-4">Loading courses...</div>;
  }

  if (error) {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger">{error}</div>
        <button onClick={() => fetchCourses()} className="btn btn-primary">
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Available Courses</h2>
      
      {/* Add Course Form for Instructors */}
      {user?.role === 'Instructor' && (
        <div className="card mb-4">
          <div className="card-body">
            <h3 className="card-title">Add New Course</h3>
            <form onSubmit={handleAddCourse}>
              <div className="mb-3">
                <label htmlFor="title" className="form-label">Title</label>
                <input
                  type="text"
                  className="form-control"
                  id="title"
                  name="title"
                  value={newCourse.title}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="mb-3">
                <label htmlFor="description" className="form-label">Description</label>
                <textarea
                  className="form-control"
                  id="description"
                  name="description"
                  value={newCourse.description}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="mb-3">
                <label htmlFor="mediaUrl" className="form-label">Media URL</label>
                <input
                  type="url"
                  className="form-control"
                  id="mediaUrl"
                  name="mediaUrl"
                  value={newCourse.mediaUrl}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <button type="submit" className="btn btn-primary">Add Course</button>
            </form>
          </div>
        </div>
      )}

      {/* Courses List */}
      <div className="row">
        {courses.length === 0 ? (
          <div className="col-12">
            <div className="alert alert-info">No courses available.</div>
          </div>
        ) : (
          courses.map(course => (
            <div key={course.courseId} className="col-md-4 mb-4">
              <div className="card h-100">
                {/* {course.mediaUrl && (
                  <img
                    src={course.mediaUrl}
                    className="card-img-top"
                    alt={course.title}
                    style={{ height: '200px', objectFit: 'cover' }}
                  />
                )} */}
                <div className="card-body">
                  <h5 className="card-title">{course.title}</h5>
                  <p className="card-text">{course.description}</p>
                  <Link 
                    to={`/courses/${course.courseId}`} 
                    className="btn btn-primary"
                    onClick={() => {
                      console.log('View Details clicked for course:', course);
                      console.log('Current auth state:', {
                        user,
                        token: localStorage.getItem('token')
                      });
                    }}
                  >
                    View Details
                  </Link>
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