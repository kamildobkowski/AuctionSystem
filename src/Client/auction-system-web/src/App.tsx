import './App.css'
import {BrowserRouter, Routes, Route} from "react-router-dom";
import {LoginPage} from "./features/login/LoginPage.tsx";
import {RegisterPage} from "./features/register/RegisterPage.tsx";
import {ConfigProvider} from "antd";

function App() {
  return (
    <>
        <ConfigProvider>
                <BrowserRouter>
                    <Routes>
                        <Route path="/login" element={<LoginPage/>}/>
                        <Route path="/register" element={<RegisterPage/>}/>
                    </Routes>
                </BrowserRouter>
        </ConfigProvider>
    </>
  )
}

export default App
