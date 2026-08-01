<script setup lang="ts">
import { Button } from '~/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '~/components/ui/card'
import { Input } from '~/components/ui/input'
import { Label } from '~/components/ui/label'

definePageMeta({ layout: 'auth' })

const { login } = useAuth()
const router = useRouter()

const email = ref('')
const password = ref('')
const isSubmitting = ref(false)
const errorMessage = ref('')

async function onSubmit() {
  errorMessage.value = ''
  isSubmitting.value = true
  try {
    await login(email.value, password.value)
    await router.push('/dashboard')
  }
  catch (error: any) {
    errorMessage.value = error?.data?.statusMessage || error?.data?.error || 'Não foi possível entrar. Verifique suas credenciais.'
  }
  finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <Card class="glow-border w-full max-w-sm transition-shadow duration-300 hover:shadow-[0_0_0_1px_theme(colors.cyan.400/20%),0_0_50px_-10px_theme(colors.cyan.400/45%)]">
    <CardHeader>
      <CardTitle class="text-2xl">
        Entrar
      </CardTitle>
      <CardDescription>Acesse sua conta do Finance Tracker</CardDescription>
    </CardHeader>
    <CardContent>
      <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
        <div class="flex flex-col gap-1.5">
          <Label for="email">E-mail</Label>
          <Input id="email" v-model="email" type="email" placeholder="voce@exemplo.com" required autocomplete="email" />
        </div>
        <div class="flex flex-col gap-1.5">
          <Label for="password">Senha</Label>
          <Input id="password" v-model="password" type="password" required autocomplete="current-password" />
        </div>
        <p v-if="errorMessage" class="text-sm text-destructive">
          {{ errorMessage }}
        </p>
        <Button type="submit" class="w-full" :disabled="isSubmitting">
          {{ isSubmitting ? 'Entrando...' : 'Entrar' }}
        </Button>
      </form>
      <p class="mt-4 text-center text-sm text-muted-foreground">
        Não tem uma conta?
        <NuxtLink to="/register" class="text-primary underline-offset-4 hover:underline">
          Cadastre-se
        </NuxtLink>
      </p>
    </CardContent>
  </Card>
</template>
