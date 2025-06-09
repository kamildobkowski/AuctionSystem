import React from "react";
import { Form, Input, Button } from "antd";
import { UserOutlined, MailOutlined, LockOutlined, HomeOutlined } from "@ant-design/icons";
import type { FormInstance } from "antd";

export interface CompanyRegistrationValues {
    companyName: string;
    companyId: string;
    username: string;
    email: string;
    password: string;
    confirm: string;
}

export const CompanyRegisterForm: React.FC = () => {
    const [form] = Form.useForm<CompanyRegistrationValues>();


    const validatePasswords = ({ getFieldValue }: FormInstance<CompanyRegistrationValues>) => ({
        validator(_: any, value: string) {
            if (!value || getFieldValue("password") === value) {
                return Promise.resolve();
            }
            return Promise.reject(new Error("Passwords do not match!"));
        }
    });

    const onFinish = (values: CompanyRegistrationValues) => {
        console.log("Company registration values:", values);
    };

    return (
        <Form form={form} layout="vertical" onFinish={onFinish} scrollToFirstError>
            <Form.Item
                name="companyName"
                label="Company Name"
                rules={[{ required: true, message: "Please enter your company name!" }]}
            >
                <Input prefix={<HomeOutlined />} placeholder="Company Name" size="large" />
            </Form.Item>

            <Form.Item
                name="companyId"
                label="Company ID"
                rules={[{ required: true, message: "Please enter your company ID!" }]}
            >
                <Input placeholder="Company ID" size="large" />
            </Form.Item>

            <Form.Item
                name="username"
                label="Username"
                rules={[{ required: true, message: 'Please enter your username!' }]}
            >
                <Input prefix={<UserOutlined />} placeholder="Username" size="large" />
            </Form.Item>

            <Form.Item
                name="email"
                label="Email"
                rules={[
                    { required: true, message: 'Please enter your email!' },
                    { type: 'email', message: 'Please enter a valid email!' }
                ]}
            >
                <Input prefix={<MailOutlined />} placeholder="Email" size="large" />
            </Form.Item>

            <Form.Item
                name="password"
                label="Password"
                rules={[
                    { required: true, message: 'Please enter your password!' },
                    { min: 8, message: 'Password must be at least 8 characters.' }
                ]}
                hasFeedback
            >
                <Input.Password prefix={<LockOutlined />} placeholder="Password" size="large" />
            </Form.Item>

            <Form.Item
                name="confirm"
                label="Confirm Password"
                dependencies={["password"]}
                hasFeedback
                rules={[
                    { required: true, message: 'Please confirm your password!' },
                    validatePasswords(form)
                ]}
            >
                <Input.Password prefix={<LockOutlined />} placeholder="Confirm Password" size="large" />
            </Form.Item>

            <Form.Item>
                <Button type="primary" htmlType="submit" block size="large">
                    Register Company
                </Button>
            </Form.Item>
        </Form>
    );
};