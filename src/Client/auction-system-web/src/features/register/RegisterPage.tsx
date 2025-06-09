import React, { useState } from "react";
import { Row, Col, Card, Typography, Segmented } from "antd";
import { UserOutlined, BankOutlined } from "@ant-design/icons";
import { PersonalRegisterForm } from "./components/PersonalRegisterForm";
import { CompanyRegisterForm } from "./components/CompanyRegisterForm";

type AccountType = "personal" | "company";

export const RegisterPage: React.FC = () => {
    const [accountType, setAccountType] = useState<AccountType>("personal");

    return (
        <Row justify="center" align="middle" style={{ minHeight: "100vh", background: "#f0f2f5" }}>
            <Col xs={22} sm={18} md={12} lg={8} xl={6}>
                <Card bordered={false} style={{ borderRadius: 8, boxShadow: "0 4px 12px rgba(0, 0, 0, 0.15)" }}>
                    <Typography.Title level={2} style={{ textAlign: "center", marginBottom: 24 }}>
                        {accountType === "personal" ? "Create Personal Account" : "Create Company Account"}
                    </Typography.Title>

                    <Segmented
                        options={[
                            { label: "Personal", icon: <UserOutlined />, value: "personal" },
                            { label: "Company", icon: <BankOutlined />, value: "company" }
                        ]}
                        block
                        value={accountType}
                        onChange={setAccountType as any}
                        style={{ marginBottom: 24 }}
                    />

                    {accountType === "personal" ? <PersonalRegisterForm /> : <CompanyRegisterForm />}

                    <Typography.Text style={{ display: "block", textAlign: "center", marginTop: 16 }}>
                        Already have an account? <a href="/login">Sign in</a>
                    </Typography.Text>
                </Card>
            </Col>
        </Row>
    );
};