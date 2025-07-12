import { useState } from 'react'
import './App.css'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import Login from './components/Login';
import Register from './components/user/Register';

function App() {
  const [count, setCount] = useState(0)

  const router = createBrowserRouter([
    {
      path : "/",
      element : <Login />
    },
    {
      path : "/user-register",
      element : <Register />
    },
  ])

  return (
    <>
        <RouterProvider router={router} />
    </>
  )
}

export default App
