<template>
  <div id="poster">
    <el-form class="login-container" label-position="left" label-width="0px" :model="loginForm" :rules="rules" ref="loginForm">
      <h3 class="login_title">注册账号</h3>
      <el-form-item prop="username">
        <el-input type="text" v-model="loginForm.username" auto-complete="off" placeholder="账号"></el-input>
      </el-form-item>
      <el-form-item prop="password">
        <el-input type="password" v-model="loginForm.password" auto-complete="off" placeholder="密码"></el-input>
      </el-form-item>
      <el-form-item prop="repassword">
        <el-input type="password" v-model="loginForm.repassword" auto-complete="off" placeholder="再次输入"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" style="width: 100%; background: #505458;border: none" v-on:click="register('loginForm')">注册</el-button>
      </el-form-item>
      <el-form-item style="width: 100%;">
        <el-link href="/#/login" :underline="false">已有账号?前往登录</el-link>
      </el-form-item>
    </el-form>
  </div>
</template>
<script>
export default {
  name: 'Register',
  data () {
    var validateUser = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('账号不能为空'))
      } else if (!this.pattern1.test(value)) {
        callback(new Error('账号格式不正确'))
      } else {
        callback()
      }
    }
    var validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('密码不能为空'))
      } else if (!this.pattern1.test(value)) {
        callback(new Error('密码格式不正确'))
      } else {
        callback()
      }
    }
    var validateRepass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('密码不能为空'))
      } else if (!this.pattern1.test(value)) {
        callback(new Error('密码格式不正确'))
      } else if (this.loginForm.password !== this.loginForm.repassword) {
        callback(new Error('两次密码不同'))
      } else {
        callback()
      }
    }
    return {
      loginForm: {
        username: '',
        password: '',
        repassword: ''
      },
      pattern1: /^[a-zA-Z0-9]{6,18}$/,
      rules: {
        username: [{
          validator: validateUser, trigger: 'blur'
        }],
        password: [{
          validator: validatePass, trigger: 'blur'
        }],
        repassword: [{
          validator: validateRepass, trigger: 'blur'
        }]
      }
    }
  },
  methods: {
    register (loginForm) {
      var username = this.loginForm.username
      var password = this.loginForm.password
      var repassword = this.loginForm.repassword
      username = encodeURIComponent(encodeURIComponent(this.$getCode(username)))
      password = encodeURIComponent(encodeURIComponent(this.$getCode(password)))
      repassword = encodeURIComponent(encodeURIComponent(this.$getCode(repassword)))
      this.$refs[loginForm].validate((valid) => {
        if (valid) {
          var li = 'product/register/' + username + '/' + password + '/' + repassword
          this.$axios({method: 'post', url: li})
            .then(response => {
              this.loginForm.username = ''
              this.loginForm.password = ''
              this.loginForm.repassword = ''
              if (response.data.Code === 200) {
                const loading = this.$loading({
                  lock: true,
                  text: '注册成功,即将跳转',
                  spinner: 'el-icon-loading',
                  background: 'rgba(0,0,0,0.7)'
                })
                setTimeout(() => {
                  loading.close()
                  this.$router.push({path: '/login'})
                }, 2000)
              } else if (response.data.Code === 400) {
                this.$message.error(response.data.Mess)
              }
            })
        } else {
          return false
        }
      })
    }
  }
}
</script>
<style scoped>
#poster {
  background:url("../assets/eva.jpg") no-repeat;
  background-position: center;
  height: 100%;
  width: 100%;
  background-size: cover;
  position: fixed;
}
body{
  margin: 0px;
}
.login-container {
  border-radius: 15px;
  background-clip: padding-box;
  margin: 90px auto;
  width: 350px;
  padding: 35px 35px 15px 35px;
  background: #fff;
  border: 1px solid #eaeaea;
  box-shadow: 0 0 25px #cac6c6;
}
.login_title {
  margin: 0px auto 40px auto;
  text-align: center;
  color: #505458;
}
</style>>
