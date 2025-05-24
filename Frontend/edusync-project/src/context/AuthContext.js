import React, { createContext, useState, useContext, useEffect, useRef } from 'react';
import axios from '../utils/axiosConfig';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const initializedRef = useRef(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (initializedRef.current) {
      console.log('Auth already initialized, skipping...');
      return;
    }

    console.log('Initializing auth state...');
    const storedToken = localStorage.getItem('token');
    console.log('Stored token:', storedToken ? 'exists' : 'not found');
    
    if (storedToken && storedToken !== 'dummy-token') {
      try {
        const decoded = jwtDecode(storedToken);
        console.log('Token decoded successfully:', decoded);
        
        if (Date.now() >= decoded.exp * 1000) {
          console.log('Token expired, logging out');
          logout();
          return;
        }
        
        const storedUser = localStorage.getItem('user');
        console.log('Stored user data:', storedUser ? 'exists' : 'not found');
        
        if (storedUser) {
          const userData = JSON.parse(storedUser);
          setUser(userData);
          axios.defaults.headers.common['Authorization'] = `Bearer ${storedToken}`;
        }
      } catch (error) {
        console.error('Error decoding token:', error);
        // Don't logout on token decode error for dummy token
        if (storedToken !== 'dummy-token') {
          logout();
        }
      }
    } else {
      console.log('No valid token found');
      setUser(null);
    }
    setLoading(false);
    initializedRef.current = true;
  }, []);

  const login = async (email, password) => {
    try {
      console.log('Starting login process for:', email);
      setError(null);
      
      // First, attempt to login
      const loginResponse = await axios.post('/Auth/login', {
        email,
        passwordHash: password
      }, {
        withCredentials: true,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      });
      console.log('Login response:', loginResponse.data);
      
      // Handle the new response format that includes token and user data
      if (loginResponse.data && loginResponse.data.token) {
        const userData = {
          token: loginResponse.data.token,
          role: loginResponse.data.role || 'Student',
          name: loginResponse.data.name || email.split('@')[0],
          email: loginResponse.data.email || email,
          id: loginResponse.data.id
        };
        
        console.log('Setting user data:', userData);
        
        // Store user data
        localStorage.setItem('user', JSON.stringify(userData));
        localStorage.setItem('token', userData.token);
        
        // Update state and axios headers
        setUser(userData);
        axios.defaults.headers.common['Authorization'] = `Bearer ${userData.token}`;
        
        console.log('User data stored and state updated');
        return userData;
      } else {
        throw new Error('Invalid login response format - missing token');
      }
    } catch (error) {
      console.error('Login error:', error);
      const errorMessage = error.response?.data?.message || 
                          error.response?.data?.title ||
                          error.message ||
                          'Login failed. Please check your credentials.';
      setError(errorMessage);
      throw error;
    }
  };

  const register = async (userData) => {
    try {
      console.log('Starting registration process for:', userData.email);
      
      // Format the data according to the backend's expectations
      const registerData = {
        name: userData.name,
        email: userData.email,
        passwordHash: userData.password,
        role: userData.role
      };

      console.log('Sending registration data:', registerData);
      const response = await axios.post('/Auth/register', registerData, {
        withCredentials: true,
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      });
      console.log('Registration response:', response.data);
      
      return response.data;
    } catch (error) {
      console.error('Registration error:', error);
      // Extract validation errors if they exist
      if (error.response?.data?.errors) {
        const validationErrors = Object.values(error.response.data.errors)
          .flat()
          .join(', ');
        throw new Error(validationErrors);
      }
      throw new Error(
        error.response?.data?.message || 
        'Failed to register. Please try again.'
      );
    }
  };

  const logout = () => {
    console.log('Logging out user');
    localStorage.clear();
    setUser(null);
    // Remove the authorization header
    delete axios.defaults.headers.common['Authorization'];
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <AuthContext.Provider value={{ user, login, logout, register, error }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
