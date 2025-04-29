import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import { Result, Button } from "antd";
import './App.css'
import {BrowserRouter, Routes, Route} from "react-router-dom";
import {LoginPage} from "./features/login/LoginPage.tsx";

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LoginPage/>}/>
            </Routes>
        </BrowserRouter>
    </>
  )
}

export default App
