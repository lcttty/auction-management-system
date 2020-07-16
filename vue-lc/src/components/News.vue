<template>
  <div>
    <el-col style="margin-top:30px;" :span="6">
      <hr style="width:25%; margin-left:0px;background-color:red;border-top:1px solid #FF9797;" />
      <ul style="margin-left:0px;text-align:left" v-for="i in data" :key="i.title">
        <el-link @click="link(i.title)" :underline="false"><li>{{i.title}}</li></el-link>
      </ul>
    </el-col>
    <el-col :span="18" style="margin-top:50px;">
      <span style="font-size:20px;">
        <div><h3>{{ title }}</h3></div>
        <div v-html="notice" style="text-align:left"></div>
      </span>
    </el-col>
  </div>
</template>
<script>
export default {
  name: 'News',
  data () {
    return {
      data: [],
      notice: '',
      title: ''
    }
  },
  methods: {
    huoqu () {
      this.$axios({method: 'post', url: 'product/notice'})
        .then(response => {
          var data = response.data.Table
          for (var i in data) {
            this.data.push(data[i])
          }
        })
    },
    link (title) {
      for (var i in this.data) {
        if (this.data[i].title === title) {
          var notice = this.data[i].notice
          this.notice = notice
          this.title = title
        }
      }
    }
  },
  mounted: function () {
    this.huoqu()
  }
}
</script>
<style scoped>

</style>
