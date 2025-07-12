
// Register.jsx

// Register.jsx

import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

const Register = () => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleRegister = async (e) => {
    e.preventDefault();
    const response = await axios.post(`${process.env.REACT_APP_BACKEND_URL}/register`, {
      name,
      email,
      password,
    });
    console.log(response.data); // Optional: redirect or message
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md p-8 space-y-6 bg-white rounded-2xl shadow-md">
        <h2 className="text-2xl font-bold text-center text-gray-800">Register</h2>
        <form onSubmit={handleRegister} className="space-y-4">
          {/* name input */}
          <div>
            <label htmlFor="name" className="block text-sm font-medium text-gray-700">
              Full Name
            </label>
            <input
              type="text"
              id="name"
              name="name"
              className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-md focus:ring-2 focus:ring-green-500 focus:outline-none"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
            />
          </div>

          {/* email input */}
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">
              Email Address
            </label>
            <input
              type="email"
              id="email"
              name="email"
              className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-md focus:ring-2 focus:ring-green-500 focus:outline-none"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          {/* password input */}
          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700">
              Password
            </label>
            <input
              type="password"
              id="password"
              name="password"
              className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-md focus:ring-2 focus:ring-green-500 focus:outline-none"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          {/* submit button */}
          <button
            type="submit"
            className="w-full py-2 font-semibold text-white bg-green-600 rounded-md hover:bg-green-700 focus:ring-2 focus:ring-green-400"
          >
            Register
          </button>

          {/* login link */}
          <p className="text-sm text-center text-gray-600">
            Already have an account?{' '}
            <Link to="/" className="text-green-600 hover:underline">
              Login here
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
};

export default Register;

