import React, { useState } from "react";
import {Form, Input, Button, Select, notification} from "antd";
import { MailOutlined, LockOutlined } from "@ant-design/icons";
import type { FormInstance } from "antd";
import {
    ApiException,
    Client,
    ErrorResult,
    RegisterPersonalUserCommand
} from "../../../api/identity/IdentityClient";
import { showServerError } from "../../../layout/onScreenNotifications/onScreenNotifications";

export interface PersonalRegistrationValues {
    email: string;
    password: string;
    RepeatPassword: string;
    firstName: string;
    lastName: string;
    phonePrefix: string;
    phoneNumber: string;
}

const prefixOptions: {
    label: string;
    value: string;
    minLength: number;
    maxLength: number;
}[] = [
    { label: "🇵🇱 +48", value: "+48", minLength: 9, maxLength: 9 },
    { label: "🇬🇧 +44", value: "+44", minLength: 10, maxLength: 10 },
    { label: "🇺🇸 +1", value: "+1", minLength: 10, maxLength: 10 },
    { label: "🇩🇪 +49", value: "+49", minLength: 10, maxLength: 11 },
    { label: "🇫🇷 +33", value: "+33", minLength: 9, maxLength: 9 }
];

const validatePhoneNumber = (form: FormInstance<PersonalRegistrationValues>) => ({
    validator(_: any, value: string) {
        const prefix = form.getFieldValue("phonePrefix");
        const option = prefixOptions.find((opt) => opt.value === prefix);
        const min = option?.minLength ?? 0;
        const max = option?.maxLength ?? Infinity;
        if (!value) {
            return Promise.reject(new Error("Please enter your phone number!"));
        }
        if (!/^\d+$/.test(value)) {
            return Promise.reject(new Error("Phone number must contain only digits."));
        }
        if (value.length < min || value.length > max) {
            return Promise.reject(
                new Error(
                    `Phone number length must be between ${min} and ${max} digits.`
                )
            );
        }
        return Promise.resolve();
    }
});

export const PersonalRegisterForm: React.FC = () => {
    const [api, contextHolder] = notification.useNotification();
    const [form] = Form.useForm<PersonalRegistrationValues>();
    const [loading, setLoading] = useState(false);
    const identityApiService = new Client(
        "http://vpn.kamildobkowski.pl:4444/api/identity"
    );

    const onFinish = async (values: PersonalRegistrationValues) => {
        setLoading(true);
        try {
            api.error({message: "test"})
            const request = new RegisterPersonalUserCommand(values);
            await identityApiService.personal(request);

        } catch (error: any) {
            if (error instanceof ApiException) {
                if (error.status === 400) {
                    const errors = (error.result as ErrorResult).errors;
                    if (errors) {
                        errors.forEach((err) => {
                            form.setFields([
                                { name: err.errorField, errors: [err.errorMessage] }
                            ]);
                        });
                    } else {
                        showServerError((error.result as ErrorResult).errorDescription);
                    }
                } else {
                    showServerError();
                }
            } else {
                showServerError();
            }
        } finally {
            setLoading(false);
        }
    };

    const selectedPrefixValue = Form.useWatch("phonePrefix", form) as string;
    const selectedOption = prefixOptions.find(
        (opt) => opt.value === selectedPrefixValue
    );
    const maxLen = selectedOption?.maxLength;

    return (
        <Form
            form={form}
            layout="vertical"
            onFinish={onFinish}
            scrollToFirstError
            initialValues={{ phonePrefix: prefixOptions[0].value }}
        >
            <Form.Item
                name="firstName"
                label="First Name"
                rules={[{ required: true, message: "Please enter your first name!" }]}
            >
                <Input placeholder="First Name" size="large" />
            </Form.Item>

            <Form.Item
                name="lastName"
                label="Last Name"
                rules={[{ required: true, message: "Please enter your last name!" }]}
            >
                <Input placeholder="Last Name" size="large" />
            </Form.Item>

            <Form.Item label="Phone" required>
                <Input.Group compact>
                    <Form.Item
                        name="phonePrefix"
                        noStyle
                        rules={[{ required: true, message: "Please select your prefix!" }]}
                    >
                        <Select
                            options={prefixOptions.map(({ label, value }) => ({ label, value }))}
                            style={{ width: "30%" }}
                            size="large"
                        />
                    </Form.Item>
                    <Form.Item
                        name="phoneNumber"
                        noStyle
                        dependencies={["phonePrefix"]}
                        rules={[validatePhoneNumber(form)]}
                    >
                        <Input
                            style={{ width: "70%" }}
                            size="large"
                            placeholder="Phone number"
                            maxLength={maxLen}
                        />
                    </Form.Item>
                </Input.Group>
            </Form.Item>

            <Form.Item
                name="email"
                label="Email"
                rules={[
                    { required: true, message: "Please enter your email!" },
                    { type: "email", message: "Please enter a valid email!" }
                ]}
            >
                <Input prefix={<MailOutlined />} placeholder="Email" size="large" />
            </Form.Item>

            <Form.Item
                name="password"
                label="Password"
                rules={[
                    { required: true, message: "Please enter your password!" },
                    { min: 8, message: "Password must be at least 8 characters." }
                ]}
                hasFeedback
            >
                <Input.Password
                    prefix={<LockOutlined />}
                    placeholder="Password"
                    size="large"
                />
            </Form.Item>

            <Form.Item
                name="repeatPassword"
                label="Confirm Password"
                dependencies={["password"]}
                hasFeedback
                rules={[
                    { required: true, message: "Please confirm your password!" },
                    {
                        validator(_: any, value: string) {
                            const pwd = form.getFieldValue("password");
                            if (!value || pwd === value) {
                                return Promise.resolve();
                            }
                            return Promise.reject(new Error("Passwords do not match!"));
                        }
                    }
                ]}
            >
                <Input.Password
                    prefix={<LockOutlined />}
                    placeholder="Confirm Password"
                    size="large"
                />
            </Form.Item>

            <Form.Item>
                <Button
                    type="primary"
                    htmlType="submit"
                    block
                    size="large"
                    loading={loading}
                    disabled={loading}
                >
                    {loading ? "Processing..." : "Register Personal"}
                </Button>
            </Form.Item>
        </Form>
    );
};