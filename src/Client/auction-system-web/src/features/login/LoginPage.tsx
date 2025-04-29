import { Row, Col, Card, Form, Input, Button, Typography } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
export function LoginPage() {
  return (
      <Row
          justify="center"
          align="middle"
          style={{ minHeight: "100vh", background: "#f0f2f5" }}
      >
          <Col xs={24} sm={16} md={12} lg={8}>
              <Card
                  bordered={false}
                  style={{
                      borderRadius: 8,
                      boxShadow: "0 4px 12px rgba(0, 0, 0, 0.15)",
                  }}
              >
                  <Typography.Title
                      level={2}
                      style={{ textAlign: "center", marginBottom: 24 }}
                  >
                      Sign In
                  </Typography.Title>
                  <Form layout="vertical">
                      <Form.Item
                          name="username"
                          rules={[{ required: true, message: "Please enter your username!" }]}
                      >
                          <Input
                              prefix={<UserOutlined />}
                              placeholder="Username"
                              size="large"
                          />
                      </Form.Item>

                      <Form.Item
                          name="password"
                          rules={[{ required: true, message: "Please enter your password!" }]}>
                          <Input.Password
                              prefix={<LockOutlined />}
                              placeholder="Password"
                              size="large"/>
                      </Form.Item>

                      <Form.Item>
                          <Button type="primary" htmlType="submit" block size="large">
                              Log In
                          </Button>
                      </Form.Item>

                      <Form.Item style={{ textAlign: "center", marginBottom: 0 }}>
                          <Typography.Text>
                              <a href="#">Forgot password?</a>
                          </Typography.Text>
                      </Form.Item>
                  </Form>
              </Card>
          </Col>
      </Row>
  );
}