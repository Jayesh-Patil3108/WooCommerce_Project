import { useState } from 'react'
import './App.css'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";

function App() {
  const [count, setCount] = useState(0)

  const router = createBrowserRouter([
    {
      path : "",
      element : <></>
    },
    {
      path : "",
      element : <></>
    },
  ])

  return (
    <>
        <RouterProvider router={router} />
    </>
  )
}

export default App
