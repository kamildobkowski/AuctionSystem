import 'antd/dist/reset.css';
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import {ConfigProvider} from "antd";

import { notification } from 'antd';

// wystaw globalnie samą funkcję error/info itp.
// Teraz w konsoli możesz wpisać `notification.error({...})`
;(window as any).notification = notification;

createRoot(document.getElementById('root')!).render(
    <ConfigProvider>
        <App />
    </ConfigProvider>
)
