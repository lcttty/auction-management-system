<template>
  <div id="poster">
    <el-form class="login-container" label-position="left" label-width="0px" :model="loginForm" :rules="rules" ref="loginForm">
      <h3 class="login_title">系统登录</h3>
    <el-form-item prop="username">
      <el-input type="text" v-model="loginForm.username" auto-complete="off" placeholder="账号"></el-input>
    </el-form-item>
    <el-form-item prop="password">
      <el-input type="password" v-model="loginForm.password" auto-complete="off" placeholder="密码"></el-input>
    </el-form-item>
    <el-form-item prop="yzm">
      <el-row>
        <el-col :span="12">
          <el-input type="text" v-model="loginForm.yzm" auto-complete="off" placeholder="验证码"></el-input>
        </el-col>
        <el-col :span="12">
          <el-link :underline="false"><img :src="url" v-on:click="yzm"></el-link>
        </el-col>
      </el-row>
    </el-form-item>
    <el-form-item style="width: 100%">
      <el-button type="primary" style="width: 100%; background: #505458;border: none" v-on:click="login('loginForm')">登录</el-button>
    </el-form-item>
    <el-form-item style="width: 100%">
      <el-link href="/#/register" :underline="false">没有账号?点击注册</el-link>
    </el-form-item>
    </el-form>
  </div>
</template>

<script>
export default {
  name: 'Login',
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
    var validateYzm = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('验证码不能为空'))
      } else if (!this.pattern2.test(value)) {
        callback(new Error('验证码格式不正确'))
      } else {
        callback()
      }
    }
    return {
      loginForm: {
        username: '',
        password: '',
        yzm: ''
      },
      url: '',
      pattern1: /^[a-zA-Z0-9]{6,18}$/,
      pattern2: /^[a-zA-Z0-9]{4}$/,
      rules: {
        username: [
          { validator: validateUser, trigger: 'blur' }
        ],
        password: [
          { validator: validatePass, trigger: 'blur' }
        ],
        yzm: [
          { validator: validateYzm, trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    login (loginForm) {
      this.$refs[loginForm].validate((valid) => {
        if (valid) {
          var username = this.loginForm.username
          var password = this.loginForm.password
          var yzm = this.loginForm.yzm
          username = encodeURIComponent(encodeURIComponent(this.$getCode(username)))
          password = encodeURIComponent(encodeURIComponent(this.$getCode(password)))
          var li = 'product/login/' + username + '/' + password + '/' + yzm
          this.$axios({method: 'post', url: li, responseType: 'arraybuffer'})
            .then(response => {
              this.loginForm.username = ''
              this.loginForm.password = ''
              this.loginForm.yzm = ''
              this.url = 'data:image/png;base64,' + btoa(
                new Uint8Array(response.data).reduce((data, byte) => data + String.fromCharCode(byte), ''))
              var message = response.headers.myself
              var sp = message.split(' ')
              var code = sp[0]
              var mess = decodeURIComponent(sp[1])
              if (code === '400') {
                this.$message.error(mess)
              } else if (code === '200') {
                const loading = this.$loading({
                  lock: true,
                  text: '登录成功,即将跳转',
                  spinner: 'el-icon-loading',
                  background: 'rgba(0,0,0,0.7)'
                })
                setTimeout(() => {
                  loading.close()
                  this.$router.push({path: '/index'})
                }, 2000)
              }
            })
        } else {
          return false
        }
      })
    },
    yzm () {
      var l1 = 'product/yanzhengma'
      this.$axios({method: 'post', url: l1, responseType: 'arraybuffer'})
        .then(response => {
          this.url = 'data:image/png;base64,' + btoa(
            new Uint8Array(response.data).reduce((data, byte) => data + String.fromCharCode(byte), ''))
        })
    }
  },
  mounted () {
    this.yzm()
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
</style>
